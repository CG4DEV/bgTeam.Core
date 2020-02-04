using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using bgTeam.Extensions;

namespace bgTeam.Core.Tests.Tests.Core.Extensions
{
    public class RegexExtensionsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegexForNullOrEmptyStringShouldReturnsNull(string str)
        {
            Assert.Null(str.Regex(""));
            Assert.Null(str.Regex(""));
        }

        [Fact]
        public void Regex()
        {
            Assert.Equal("I am John, i am 7 years old", ("I am John, i am 7 years old, i am from USA").Regex(@"I am (\w*), i am (\w*) years old"));
        }

        [Fact]
        public void RegexWithGroupShouldReturnsMatchGroup()
        {
            Assert.Equal("John", ("I am John, i am 7 years old, i am from USA").Regex(@"I am (\w*), i am (\w*) years old", "1"));
            Assert.Equal("7", ("I am John, i am 7 years old, i am from USA").Regex(@"I am (\w*), i am (\w*) years old", "2"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegexAllForNullOrEmptyStringShouldReturnsNull(string str)
        {
            Assert.Null(str.RegexAll(""));
            Assert.Null(str.RegexAll(""));
        }

        [Fact]
        public void RegexAll()
        {
            Assert.Equal("number: 1number: 2", ("number: 1, number: 2, string: 3").RegexAll(@"number: (\w)"));
        }
    }
}
