using bgTeam.Extensions;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Core.Extensions
{
    public class ArgumentsExtensionsTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckNull(string str)
        {
            Assert.Throws<ArgumentNullException>(() => str.CheckNull(str));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckNullOrEmpty(string str)
        {
            Assert.Throws<ArgumentNullException>(() => str.CheckNullOrEmpty(str));
        }

        [Fact]
        public void CheckNullForThis()
        {
            Assert.Throws<ArgumentNullException>(() => ((Exception)null).CheckNull("Hey"));
        }
    }
}
