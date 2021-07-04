using System;
using System.Collections.Generic;
using System.Reflection;

namespace CLI.CommandHandler
{
    public interface ICommandTypeFactory
    {
        Type[] GetAllCommandTypes(IList<AssemblyName> assemblyNames);
        Type[] GetAllCommandTypes(AssemblyName assemblyName);
    }
}