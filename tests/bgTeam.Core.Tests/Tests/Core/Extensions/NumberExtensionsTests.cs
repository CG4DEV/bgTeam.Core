using bgTeam.Extensions;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Core.Extensions
{
    public class NumberExtensionsTests
    {
        [Fact]
        public void RoundUp()
        {
            var dec = 2.934432m;

            var rounded = dec.RoundUp(3);
            Assert.Equal(2.935m, rounded);

            rounded = dec.RoundUp(1);
            Assert.Equal(3m, rounded);
        }
    }
}
