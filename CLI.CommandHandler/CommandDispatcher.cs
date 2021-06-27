using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CLI.CommandHandler.Abstractions;
using Microsoft.Extensions.Logging;


[assembly: InternalsVisibleTo("CLI.CommandHandler.Tests")]
namespace CLI.CommandHandler
{
    public class CommandDispatcher
    {
        public ILogger<CommandDispatcher>? Logger { get; set; }

        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public CommandDispatcher(AssemblyName assemblyName) : this(new List<AssemblyName> {assemblyName})
        {
        }

        public CommandDispatcher(IList<AssemblyName> assemblyNames)
        {
            _commandHandlerFactory = new CommandHandlerFactory(assemblyNames, new TypeFinder());
        }
        
        internal CommandDispatcher(ICommandHandlerFactory commandHandlerFactory)
        {
            _commandHandlerFactory = commandHandlerFactory;
        }

        public Task DispatchAsync(object command)
        {
            var commandHandlerType = _commandHandlerFactory.GetCommandHandlerType(command);
            if (commandHandlerType == null)
                throw new ArgumentException($"could not find handler for type {command.GetType()}");
            
            var handler = Activator.CreateInstance(commandHandlerType);
            var method = commandHandlerType.GetMethod("RunAsync")!;
            
            return ((Task) method.Invoke(handler, new[] {command}))!;
        }
    }
}