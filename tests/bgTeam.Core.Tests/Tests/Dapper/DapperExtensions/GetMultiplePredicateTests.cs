using bgTeam.Core.Tests.Infrastructure;
using DapperExtensions;
using System.Linq;
using Xunit;

namespace bgTeam.Core.Tests.Dapper.DapperExtensions
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
