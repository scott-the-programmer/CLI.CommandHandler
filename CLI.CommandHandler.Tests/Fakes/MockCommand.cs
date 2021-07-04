using System;
using CLI.CommandHandler.Abstractions;

namespace CLI.CommandHandler.Tests.Fakes
{
    public class MockCommand : ICommand
    {
        
    }
    
    public class MockCommand2 : ICommand
    {
        
    }

    public class OverSubscribedCommand : ICommand
    {
        
    }

    public class IsolatedCommand : ICommand
    {
        
    }

    public class ExceptionCommand : ICommand
    {
        public Exception ExceptionToThrow { get; set; } = new Exception("I am an exception");
    }
}