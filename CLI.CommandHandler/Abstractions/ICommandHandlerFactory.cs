using System;

namespace CLI.CommandHandler.Abstractions
{
    internal interface ICommandHandlerFactory
    {
        Type? GetCommandHandlerType(object command);
    }
}