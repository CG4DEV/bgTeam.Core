using System;
using System.Collections.Generic;
using bgTeam.Extensions;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core.Extensions
{
    public class LinqExtensionsTests
    {

        [Fact]
        public void NullOrEmpty()
        {
            var list = new List<string>();
            Assert.True(list.NullOrEmpty());
            Assert.True(((IEnumerable<string>)null).NullOrEmpty());
        }

        [Fact]
        public void DistinctBy()
        {
            var list = new List<string>() { "Hey", "Bye", "Hey" };
            var result = list.DistinctBy(x => x.Length == 3);
            var str = Assert.Single(result);
            Assert.Equal("Hey", str);
        }
    }
}
