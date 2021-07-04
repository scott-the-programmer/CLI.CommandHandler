using System;
using System.Collections.Generic;
using System.Reflection;
using CLI.CommandHandler.Abstractions;
using CLI.CommandHandler.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace CLI.CommandHandler.Tests
{
    public class CommandDispatcherTests
    {
        [Test]
        public void should_dispatch_command()
        {
            // Arrange
            var mockCommandFactor = new Mock<ICommandHandlerFactory>();
            mockCommandFactor.Setup(m => m.GetCommandHandlerType(It.IsAny<object>()))
                .Returns(typeof(MockCommandHandler));

            var command = new MockCommand();

            var commandDispatcher = new CommandDispatcher(mockCommandFactor.Object);

            // Act
            var task = commandDispatcher.DispatchAsync(command);

            // Assert
            Assert.That(task.IsCompletedSuccessfully);
        }

        [Test]
        public void should_throw_dispatch_exception_if_no_handlers_are_found()
        {
            // Arrange
            var mockCommandFactor = new Mock<ICommandHandlerFactory>();
            mockCommandFactor.Setup(m => m.GetCommandHandlerType(It.IsAny<object>()))
                .Returns((Type?) null);

            var command = new MockCommand();

            var commandDispatcher = new CommandDispatcher(mockCommandFactor.Object);

            // Act & Assert
            Assert.Throws<CommandDispatchException>(() => commandDispatcher.DispatchAsync(command));
        }
        
        [Test]
        public void should_throw_dispatch_exception_if_handler_throws()
        {
            // Arrange
            var mockCommandFactor = new Mock<ICommandHandlerFactory>();
            mockCommandFactor.Setup(m => m.GetCommandHandlerType(It.IsAny<object>()))
                .Returns(typeof(ExceptionCommandHandler));

            var command = new ExceptionCommand();

            var commandDispatcher = new CommandDispatcher(mockCommandFactor.Object);

            // Act & Assert
            Assert.ThrowsAsync<CommandDispatchException>(() => commandDispatcher.DispatchAsync(command));
        }
        
        [Test]
        public void handler_exception_should_include_inner_exception()
        {
            // Arrange
            var mockCommandFactor = new Mock<ICommandHandlerFactory>();
            mockCommandFactor.Setup(m => m.GetCommandHandlerType(It.IsAny<object>()))
                .Returns(typeof(ExceptionCommandHandler));

            var command = new ExceptionCommand();

            var commandDispatcher = new CommandDispatcher(mockCommandFactor.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<CommandDispatchException>(() => commandDispatcher.DispatchAsync(command));
            
            Assert.Multiple(() =>
            {
                Assert.That(exception.InnerException, Is.Not.Null);
                var innerException = exception.InnerException;
                Assert.That(innerException!.Message, Is.EqualTo("I am an exception"));
            });
        }
        

        [Test]
        public void should_construct() // Superfluous in nature but we want to at least smoke test public constructors 
        {
            // Arrange, Act & Assert
            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() =>
                {
                    var _ = new CommandDispatcher(Assembly.GetExecutingAssembly().GetName());
                });
                Assert.DoesNotThrow(() =>
                {
                    var _ = new CommandDispatcher(new List<AssemblyName> {Assembly.GetExecutingAssembly().GetName()});
                });
            });
        }
    }
}