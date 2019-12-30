using DapperExtensions;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.DapperExtensions
{
   public class PredicateTests
   {
        [Fact]
        public void Field()
        {
            var predicate = Predicates.Field<TestEntity>(x => x.Id, Operator.Eq, 2, true);
            Assert.True(predicate.Not);
            Assert.Equal(Operator.Eq, predicate.Operator);
            Assert.Equal("Id", predicate.PropertyName);
            Assert.Equal(2, predicate.Value);
        }

        [Fact]
        public void Property()
        {
            var predicate = Predicates.Property<TestEntity, TestEntity>(x => x.Id, Operator.Eq, y => y.Name, true);
            Assert.True(predicate.Not);
            Assert.Equal(Operator.Eq, predicate.Operator);
            Assert.Equal("Id", predicate.PropertyName);
            Assert.Equal("Name", predicate.PropertyName2);
        }

        [Fact]
        public void Group()
        {
            var predicate = Predicates.Group(GroupOperator.And, new FieldPredicate<TestEntity>());
            Assert.Equal(GroupOperator.And, predicate.Operator);
            Assert.Single(predicate.Predicates);
        }

        [Fact]
        public void Exists()
        {
            var fieldPredicate = new FieldPredicate<TestEntity>();
            var predicate = Predicates.Exists<TestEntity>(fieldPredicate, true);
            Assert.True(predicate.Not);
            Assert.Same(predicate.Predicate, fieldPredicate);
        }

        [Fact]
        public void Between()
        {
            var fieldPredicate = new FieldPredicate<TestEntity>();
            var betweenValues = new BetweenValues { Value1 = 10, Value2 = 20 };
            var predicate = Predicates.Between<TestEntity>(x => x.Id, new BetweenValues { Value1 = 10, Value2 = 20 }, true);
            Assert.True(predicate.Not);
            Assert.Equal(betweenValues, predicate.Value);
            Assert.Equal("Id", predicate.PropertyName);
        }
    }
}
