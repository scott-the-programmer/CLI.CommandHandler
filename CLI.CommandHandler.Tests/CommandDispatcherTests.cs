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
        public void should_throw_arg_exception_if_no_handlers_are_found()
        {
            // Arrange
            var mockCommandFactor = new Mock<ICommandHandlerFactory>();
            mockCommandFactor.Setup(m => m.GetCommandHandlerType(It.IsAny<object>()))
                .Returns((Type?) null);

            var command = new MockCommand();

            var commandDispatcher = new CommandDispatcher(mockCommandFactor.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => commandDispatcher.DispatchAsync(command));
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