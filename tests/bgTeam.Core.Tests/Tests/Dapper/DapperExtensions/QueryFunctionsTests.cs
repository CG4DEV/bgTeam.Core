using DapperExtensions.Builder;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Dapper.DapperExtensions
{
    public class QueryFunctionsTests
    {
        [Fact]
        public void Like()
        {
            Assert.Throws<InvalidOperationException>(() => QueryFunctions.Like(@"\w+", "string"));
        }
    }
}
