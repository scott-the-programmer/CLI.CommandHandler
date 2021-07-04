using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CLI.CommandHandler.Abstractions;

namespace CLI.CommandHandler
{
    public class CommandTypeFactory : ICommandTypeFactory
    {
        private readonly ITypeFinder _typeFinder;

        public CommandTypeFactory(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }


        public Type[] GetAllCommandTypes(AssemblyName assemblyName)
        {
            return GetAllCommandTypes(new List<AssemblyName> {assemblyName});
        }

        public Type[] GetAllCommandTypes(IList<AssemblyName> assemblyNames)
        {
            IEnumerable<Type> commandTypes = new List<Type>();

            foreach (var name in assemblyNames)
            {
                var types = _typeFinder.FindAllTypesFor(Assembly.Load(name));
                var filteredTypes = types.Where(type => type.IsAssignableTo(typeof(ICommand)));
                commandTypes = commandTypes.Concat(filteredTypes);
            }

            return commandTypes.ToArray();
        }
    }
}