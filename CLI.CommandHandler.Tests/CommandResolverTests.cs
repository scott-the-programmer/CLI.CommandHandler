using System;
using System.Collections.Generic;
using System.Reflection;
using CLI.CommandHandler.Abstractions;
using CLI.CommandHandler.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace CLI.CommandHandler.Tests
{
    public class CommandResolverTests
    {
        private readonly AssemblyName _executingAssemblyName;

        public CommandResolverTests()
        {
            _executingAssemblyName = Assembly.GetExecutingAssembly().GetName();
        }

        [Test]
        public void should_find_command_handler_given_correct_command()
        {
            // Arrange
            var mockTypeFinder = new Mock<ITypeFinder>();
            mockTypeFinder.Setup(m => m.FindAllTypesFor(It.IsAny<Assembly>()))
                .Returns(new List<Type> {typeof(MockCommandHandler), typeof(MockCommand)});
            var commandHandlerFactory =
                new CommandHandlerFactory(_executingAssemblyName, mockTypeFinder.Object);

            var command = new MockCommand();

            // Act
            var handler = commandHandlerFactory.GetCommandHandlerType(command);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(handler, Is.Not.Null);
                Assert.That(handler!.Name, Is.EqualTo(nameof(MockCommandHandler)));
            });
        }

        [Test]
        public void should_fail_to_find_isolated_command()
        {
            // Arrange
            var mockTypeFinder = new Mock<ITypeFinder>();
            mockTypeFinder.Setup(m => m.FindAllTypesFor(It.IsAny<Assembly>()))
                .Returns(new List<Type> {typeof(MockCommandHandler), typeof(MockCommand)});
            var commandHandlerFactory =
                new CommandHandlerFactory(_executingAssemblyName, mockTypeFinder.Object);

            var command = new IsolatedCommand();

            // Act
            var handler = commandHandlerFactory.GetCommandHandlerType(command);

            // Assert
            Assert.That(handler, Is.Null);
        }

        [Test]
        public void should_return_first_handler_given_multiple_handlers()
        {
            // Arrange
            var mockTypeFinder = new Mock<ITypeFinder>();
            mockTypeFinder.Setup(m => m.FindAllTypesFor(It.IsAny<Assembly>()))
                .Returns(new List<Type>
                {
                    typeof(DuplicateMockCommandHandler), typeof(DuplicateMockCommandHandler2),
                    typeof(OverSubscribedCommand)
                });
            var commandHandlerFactory =
                new CommandHandlerFactory(_executingAssemblyName, mockTypeFinder.Object);

            var command = new OverSubscribedCommand();

            // Act
            var handler = commandHandlerFactory.GetCommandHandlerType(command);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(handler, Is.Not.Null);
                Assert.That(handler!.Name, Is.EqualTo(nameof(DuplicateMockCommandHandler)));
            });
        }

        [Test]
        public void should_throw_given_no_assemblies()
        {
            // Arrange
            var mockTypeFinder = new Mock<ITypeFinder>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ =
                    new CommandHandlerFactory(new List<AssemblyName>(), mockTypeFinder.Object);
            });
        }
    }
}