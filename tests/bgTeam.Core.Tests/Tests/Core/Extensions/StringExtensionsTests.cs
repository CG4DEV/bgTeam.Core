using System;
using bgTeam.Extensions;
using System.Text;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ToStr()
        {
            Assert.Null("".ToStr());
            Assert.Null(((string)null).ToStr());
            Assert.Equal("hi", "hi".ToStr());
        }

        [Fact]
        public void ToInt()
        {
            Assert.Null("fd".ToInt());
            Assert.Null("".ToInt());
            Assert.Null(((string)null).ToInt());
            Assert.Equal(23, "23".ToInt());
            Assert.Equal(33, "fd".ToInt(33));
        }

        [Fact]
        public void ToFloat()
        {
            Assert.Null("daqwq".ToFloat());
            Assert.Null("".ToFloat());
            Assert.Null(((string)null).ToFloat());
            Assert.Equal(23.172f, "23.172".ToFloat());
            Assert.Equal(16.2f, "fd".ToFloat(16.2f));
        }

        [Fact]
        public void ToDateTime()
        {
            var dt = "2020.01.16 11:05:10";
            Assert.Null("".ToDateTime());
            Assert.Null(((string)null).ToFloat());
            Assert.Equal(DateTime.Parse(dt), dt.ToDateTime());
        }

        [Fact]
        public void ToExactDateTime()
        {
            var dt = "2020/01/16 11.05.10";
            Assert.Null("".ToExactDateTime("yyyy/MM/dd hh:mm:ss"));
            Assert.Null(((string)null).ToExactDateTime("yyyy/MM/dd hh:mm:ss"));
            Assert.Equal(new DateTime(2020, 01, 16, 11, 05, 10), dt.ToExactDateTime("yyyy/MM/dd hh.mm.ss"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FirstLetterToUpperShouldThrowsExceptionIdStringIsNullOrEmpty(string str)
        {
            Assert.Throws<ArgumentException>("str", () =>
            {
                str.FirstLetterToUpper();
            });
        }

        [Fact]
        public void FirstLetterToUpper()
        {
            Assert.Equal("Hi" , "hi".FirstLetterToUpper());
        }
    }
}
