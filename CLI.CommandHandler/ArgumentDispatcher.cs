using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CommandLine;

[assembly: InternalsVisibleTo("CLI.CommandHandler.Tests")]

namespace CLI.CommandHandler
{
    public class ArgumentDispatcher
    {
        private readonly IList<AssemblyName> _assemblyNames;
        private readonly ICommandTypeFactory _commandTypeFactory;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly string[] _args;

        public ArgumentDispatcher(string[] args) : this(args, new[] {Assembly.GetExecutingAssembly().GetName()})
        {
        }

        public ArgumentDispatcher(string[] args, string[] assemblies) : this(args,
            Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                .Where(assembly => assemblies.Any(n => n == assembly.Name)).ToList())
        {
        }


        internal ArgumentDispatcher(string[] args, IList<AssemblyName> assemblies, ICommandTypeFactory commandTypeFactory,
            ICommandDispatcher dispatcher)
        {
            _assemblyNames = assemblies;
            _args = args;
            _commandTypeFactory = commandTypeFactory;
            _commandDispatcher = dispatcher;
        }

        public ArgumentDispatcher(string[] args, IList<AssemblyName> assemblies)
        {
            _assemblyNames = assemblies;
            _args = args;
            _commandTypeFactory = new CommandTypeFactory(new TypeFinder());
            _commandDispatcher = new CommandDispatcher(assemblies);
        }

        public void Dispatch()
        {
            var commandTypes = _commandTypeFactory.GetAllCommandTypes(_assemblyNames);
            Parser.Default.ParseArguments(_args, commandTypes).WithParsed(Invoke);
        }

        internal void Invoke(object command)
        {
            _commandDispatcher.DispatchAsync(command).GetAwaiter().GetResult();
        }
    }
}