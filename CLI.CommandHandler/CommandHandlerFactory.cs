using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CLI.CommandHandler.Abstractions;
using CLI.CommandHandler.Extensions;
using Microsoft.Extensions.Logging;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

[assembly: InternalsVisibleTo("CLI.CommandHandler.Tests")]

namespace CLI.CommandHandler
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        public ILogger<CommandHandlerFactory>? Logger { get; set; }

        private readonly IList<AssemblyName> _assemblyNames;
        private readonly ITypeFinder _typeFinder;

        /// <summary>
        /// Factory that will return the corresponding ICommandHandler type implementation to ICommand
        /// <code>
        ///     var commandHandlerFactory = CommandHandlerFactory&lt;MyCommand&gt;(myAssembly);
        ///     var myCommandHandlerType = commandHandlerFactory.GetCommandHandler();
        /// </code>
        /// </summary>
        /// <param name="assemblyName"></param>
        public CommandHandlerFactory(AssemblyName assemblyName) : this(
            new List<AssemblyName> {assemblyName}, new TypeFinder())
        {
        }

        /// <summary>
        /// Factory that will return the corresponding ICommandHandler type implementation to ICommand
        /// <code>
        ///     var commandHandlerFactory = CommandHandlerFactory&lt;MyCommand&gt;(myAssemblies);
        ///     var myCommandHandlerType = commandHandlerFactory.GetCommandHandler();
        /// </code>
        /// </summary>
        /// <param name="assemblyNames"></param>
        public CommandHandlerFactory(List<AssemblyName> assemblyNames) : this(
            assemblyNames, new TypeFinder())
        {
        }

        internal CommandHandlerFactory(IList<AssemblyName> assemblyNames, ITypeFinder typeFinder)
        {
            if (assemblyNames == null || assemblyNames.Count == 0)
                throw new ArgumentNullException(nameof(assemblyNames));
            _assemblyNames = assemblyNames;
            _typeFinder = typeFinder;
        }

        internal CommandHandlerFactory(AssemblyName assemblyName, ITypeFinder typeFinder) : this(
            new List<AssemblyName> {assemblyName}, typeFinder)
        {
        }


        public Type? GetCommandHandlerType(object command)
        {
            foreach (var assemblyName in _assemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                var commandType = command.GetType();
                if (commandType == null)
                    continue;
                var commandHandler = GetCommandHandlerType(commandType, assembly);
                if (commandHandler != null)
                    return commandHandler!;
            }

            return null;
        }

        private Type? GetCommandHandlerType(Type commandType, Assembly assembly)
        {
            var commandHandlerTypes = _typeFinder.FindAllTypesFor(assembly)
                .Where(t => !t.IsInterface && t.IsGenericallyAssignableTo(typeof(ICommandHandler<>), commandType))
                .ToList();

            if (commandHandlerTypes.Count > 1)
                Logger?.LogWarning($"Multiple handlers found for {nameof(commandType)}");

            return commandHandlerTypes.FirstOrDefault();
        }
    }
}