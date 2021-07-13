using System;
using System.Threading.Tasks;
using CLI.CommandHandler.Abstractions;

namespace CLI.CommandHandler.Tests.Fakes
{
    public class MockCommandHandler : ICommandHandler<MockCommand>
    {
        public Task RunAsync(MockCommand command)
        {
            return Task.CompletedTask;
        }
    }
    
    public class MockCommandHandler2 : ICommandHandler<MockCommand2>
    {
        public Task RunAsync(MockCommand2 command)
        {
            return Task.CompletedTask;
        }
    }

    public class DuplicateMockCommandHandler : ICommandHandler<OverSubscribedCommand>
    {
        public Task RunAsync(OverSubscribedCommand command)
        {
            return Task.CompletedTask;
        } 
    }
    
    public class DuplicateMockCommandHandler2 : ICommandHandler<OverSubscribedCommand>
    {
        public Task RunAsync(OverSubscribedCommand command)
        {
            return Task.CompletedTask;
        } 
    }
    
    public class ExceptionCommandHandler : ICommandHandler<ExceptionCommand>
    {
        public Task RunAsync(ExceptionCommand command)
        {
            throw command.ExceptionToThrow;
        } 
    }

    public class BarkCommandHandler : ICommandHandler<BarkCommand>
    {
        public Task RunAsync(BarkCommand command)
        {
            Console.WriteLine(command.Noise);
            return Task.CompletedTask;
        }
    }
}