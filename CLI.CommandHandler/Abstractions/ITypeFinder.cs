using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CLI.CommandHandler.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CLI.CommandHandler.Abstractions
{
    /// <summary>
    /// Simple abstractions that defines finding types for a given
    /// assembly
    /// </summary>
    internal interface ITypeFinder
    {
        /// <summary>
        /// Retrieves types for a given assembly
        /// <code>
        /// Types[] types = TypeFinderImpl.FindTypesFor(myAssembly);
        /// </code>
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<Type> FindAllTypesFor(Assembly assembly);
    }
}