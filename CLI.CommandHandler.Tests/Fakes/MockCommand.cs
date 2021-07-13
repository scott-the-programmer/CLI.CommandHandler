using System;
using CLI.CommandHandler.Abstractions;
using CommandLine;

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
    
    [Verb("bark")]
    public class BarkCommand : ICommand
    {
        [Option('n', "noise", Required = true)]
        public string Noise { get; set; }
    }
}