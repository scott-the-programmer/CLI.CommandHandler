using System;
using System.Collections.Generic;
using System.Reflection;
using CLI.CommandHandler.Abstractions;
using CLI.CommandHandler.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace CLI.CommandHandler.Tests
{
    public class CommandTypeFactoryTests
    {
        [Test]
        public void should_find_all_mock_command_types()
        {
            // Arrange
            var mockTypeFinder = new Mock<ITypeFinder>();
            mockTypeFinder.Setup(m => m.FindAllTypesFor(It.IsAny<Assembly>()))
                .Returns(new List<Type>
                {
                    typeof(MockCommand), typeof(MockCommand2)
                });

            var commandTypeFactory = new CommandTypeFactory(mockTypeFinder.Object);
            
            // Act
            var types = commandTypeFactory.GetAllCommandTypes(Assembly.GetExecutingAssembly().GetName());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(types, Has.Length.EqualTo(2));
                Assert.That(types[0], Is.EqualTo(typeof(MockCommand)));
            });
        } 
        
        [Test]
        public void should_not_return_non_command_types()
        {
            // Arrange
            var mockTypeFinder = new Mock<ITypeFinder>();
            mockTypeFinder.Setup(m => m.FindAllTypesFor(It.IsAny<Assembly>()))
                .Returns(new List<Type>
                {
                    typeof(MockCommandHandler)
                });

            var commandTypeFactory = new CommandTypeFactory(mockTypeFinder.Object);
            
            // Act
            var types = commandTypeFactory.GetAllCommandTypes(Assembly.GetExecutingAssembly().GetName());

            // Assert
            Assert.That(types, Has.Length.EqualTo(0));
        } 
    }
}