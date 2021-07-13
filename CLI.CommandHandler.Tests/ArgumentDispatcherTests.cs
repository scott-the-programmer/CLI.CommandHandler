using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CLI.CommandHandler.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace CLI.CommandHandler.Tests
{
    public class ArgumentDispatcherTests
    {
        [Test]
        public void should_dispatch_arguments_to_command_handler()
        {
            // Arrange
            var mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(o => o.DispatchAsync(It.IsAny<object>())).Returns(Task.CompletedTask);
            var mockCommandFactory = new Mock<ICommandTypeFactory>();
            mockCommandFactory.Setup(o
                    => o.GetAllCommandTypes(It.IsAny<IList<AssemblyName>>()))
                .Returns(new[] {typeof(BarkCommand)});
            
            var argumentDispatcher = new ArgumentDispatcher(new[] {"bark", "--noise", "woof"},
                new[] {Assembly.GetExecutingAssembly().GetName()}
                , mockCommandFactory.Object, mockDispatcher.Object);

            // Act
            argumentDispatcher.Dispatch();

            // Assert
            mockDispatcher.Verify(m => m.DispatchAsync(It.IsAny<object>()), Times.Once);
        }
        
        [Test]
        public void should_not_dispatch_misaligned_arguments_to_command_handler()
        {
            // Arrange
            var mockDispatcher = new Mock<ICommandDispatcher>();
            mockDispatcher.Setup(o => o.DispatchAsync(It.IsAny<object>())).Returns(Task.CompletedTask);
            var mockCommandFactory = new Mock<ICommandTypeFactory>();
            mockCommandFactory.Setup(o
                    => o.GetAllCommandTypes(It.IsAny<IList<AssemblyName>>()))
                .Returns(new[] {typeof(BarkCommand)});
            
            var argumentDispatcher = new ArgumentDispatcher(new[] {"meow", "--noise", "meow"},
                new[] {Assembly.GetExecutingAssembly().GetName()}
                , mockCommandFactory.Object, mockDispatcher.Object);

            // Act
            argumentDispatcher.Dispatch();

            // Assert
            mockDispatcher.Verify(m => m.DispatchAsync(It.IsAny<object>()), Times.Never);
        }
    }
}