using DapperExtensions;
using System.Linq;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.DapperExtensions
{
    public class GetMultiplePredicateTests
    {
        [Fact]
        public void Add()
        {
            var predicate = new GetMultiplePredicate();
            predicate.Add<TestEntity>(new FieldPredicate<TestEntity>());
            predicate.Add<TestEntity>(12);
            Assert.Equal(2, predicate.Items.Count());
        }
    }
}
