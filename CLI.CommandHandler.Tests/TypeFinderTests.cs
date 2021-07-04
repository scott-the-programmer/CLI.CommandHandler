using System.Reflection;
using NUnit.Framework;

namespace CLI.CommandHandler.Tests
{
    public class TypeFinderTests
    {
        // Superfluous test to ensure TypeFinder does not blow up on execution.
        // Not much value testing the result here (safe to assume .NET returns the correct result)
        [Test]
        public void should_not_flame_out_when_finding_types()
        {
            // Arrange
            var typeFinder = new TypeFinder();

            // Act & Assert
            Assert.DoesNotThrow(() => typeFinder.FindAllTypesFor(Assembly.GetExecutingAssembly()));
        }
    }
}