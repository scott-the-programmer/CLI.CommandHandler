using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using CLI.CommandHandler.Abstractions;

[assembly: InternalsVisibleTo("CLI.CommandHandler.Tests")]
namespace CLI.CommandHandler
{
    internal class TypeFinder : ITypeFinder
    {
        public IEnumerable<Type> FindAllTypesFor(Assembly assembly)
        {
            return assembly.GetTypes();
        }
    }
}