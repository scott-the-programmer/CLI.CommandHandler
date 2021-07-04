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
            var (commandHandlerType, handler) = GetCommandHandlerFrom(command);

            Task taskResult;
            try
            {
                var method = commandHandlerType.GetMethod("RunAsync")!;
                taskResult = ((Task) method.Invoke(handler, new[] {command}))!;
            }
            catch (TargetInvocationException e)
            {
                var commandDispatchErrorMsg = $"{commandHandlerType} threw exception";
                if (e.InnerException == null)
                    throw new CommandDispatchException(commandDispatchErrorMsg);
                throw new CommandDispatchException(commandDispatchErrorMsg, e.InnerException!);
            }

            return taskResult;
        }

        private (Type, object) GetCommandHandlerFrom(object command)
        {
            Type commandHandlerType;
            object handler;
            try
            {
                commandHandlerType = _commandHandlerFactory.GetCommandHandlerType(command) ??
                                     throw new CommandDispatchException(
                                         $"could not find handler for type {command.GetType()}");
                handler = Activator.CreateInstance(commandHandlerType) ??
                          throw new CommandDispatchException($"Could instantiate {commandHandlerType}");
            }
            catch (Exception e)
            {
                throw new CommandDispatchException($"Could not dispatch {command.GetType()}", e);
            }

            return (commandHandlerType, handler);
        }
    }
}