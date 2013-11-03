using KnockoutGenerator.Core.Extensions;
using NUnit.Framework;

namespace KnockoutGenerator.UnitTests.Tests
{
    [TestFixture]
    public class ExtensionTests
    {
        [TestCase("MyAwesomeProperty", "myAwesomeProperty")]
        [TestCase("M", "m")]
        public void TestCamelCaseFormating(string property, string expected)
        {
            var camelCase = property.FormatCamelCase();

            Assert.AreEqual(expected, camelCase);
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ExpextArgumentNullException()
        {
            "".FormatCamelCase();
        }
    }
}
