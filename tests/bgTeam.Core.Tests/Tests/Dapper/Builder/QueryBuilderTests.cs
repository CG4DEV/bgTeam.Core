using bgTeam.Core.Tests.Infrastructure;
using DapperExtensions;
using DapperExtensions.Builder;
using System;
using System.Reflection;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Dapper.Builder
{
    public class QueryBuilderTests
    {
        [Fact]
        public void OperatorEquals()
        {
            var queryBuilder = new QueryBuilder<TestEntity>(GroupOperator.And);
            var query = queryBuilder.Equals(x => x.Name, "Hi");

            var predicateGroup = GetGroupFromQuery(query);
            var predicate = Assert.Single(predicateGroup.Predicates) as FieldPredicate<TestEntity>;
            Assert.Equal("Name", predicate.PropertyName);
            Assert.Equal("Hi", predicate.Value);
            Assert.False(predicate.Not);
            Assert.Equal(Operator.Eq, predicate.Operator);

            /*var query = queryBuilder.Equals(x => x.Name, "Hi")
                .GreaterThan(x => x.Id, 3)
                .In(x => x.Id, new int?[] { 5, 17 })
                .LessThan(x => x.Id, 20)
                .GreaterThan(x => x.Id, 30);*/
        }

        [Fact]
        public void OperatorNotEquals()
        {
            var queryBuilder = new QueryBuilder<TestEntity>(GroupOperator.And);
            var query = queryBuilder.NotEquals(x => x.Name, "Hi");

            var predicateGroup = GetGroupFromQuery(query);
            var predicate = Assert.Single(predicateGroup.Predicates) as FieldPredicate<TestEntity>;
            Assert.Equal("Name", predicate.PropertyName);
            Assert.Equal("Hi", predicate.Value);
            Assert.True(predicate.Not);
            Assert.Equal(Operator.Eq, predicate.Operator);
        }

        [Fact]
        public void LessThan()
        {
            var queryBuilder = new QueryBuilder<TestEntity>(GroupOperator.And);
            var query = queryBuilder.LessThan(x => x.Id, 16);
            var predicateGroup = GetGroupFromQuery(query);
            var predicate = Assert.Single(predicateGroup.Predicates) as FieldPredicate<TestEntity>;
            Assert.Equal("Id", predicate.PropertyName);
            Assert.Equal(16, predicate.Value);
            Assert.False(predicate.Not);
            Assert.Equal(Operator.Lt, predicate.Operator);
        }

        [Fact]
        public void GreaterThan()
        {
            var queryBuilder = new QueryBuilder<TestEntity>(GroupOperator.And);
            var query = queryBuilder.GreaterThan(x => x.Id, 16);
            var predicateGroup = GetGroupFromQuery(query);
            var predicate = Assert.Single(predicateGroup.Predicates) as FieldPredicate<TestEntity>;
            Assert.Equal("Id", predicate.PropertyName);
            Assert.Equal(16, predicate.Value);
            Assert.False(predicate.Not);
            Assert.Equal(Operator.Gt, predicate.Operator);
        }

        [Fact]
        public void In()
        {
            var queryBuilder = new QueryBuilder<TestEntity>(GroupOperator.And);
            var query = queryBuilder.In(x => x.Id, new int?[] { 17 });
            var predicateGroup = GetGroupFromQuery(query);
            var predicate = Assert.Single(predicateGroup.Predicates) as FieldPredicate<TestEntity>;
            Assert.Equal("Id", predicate.PropertyName);
            Assert.Single((int?[])predicate.Value);
            Assert.False(predicate.Not);
            Assert.Equal(Operator.Eq, predicate.Operator);
        }

        [Fact]
        public void Like()
        {
            var queryBuilder = new QueryBuilder<TestEntity>(GroupOperator.And);
            var query = queryBuilder.Like(x => x.Name, "ee");
            var predicateGroup = GetGroupFromQuery(query);
            var predicate = Assert.Single(predicateGroup.Predicates) as FieldPredicate<TestEntity>;
            Assert.Equal("Name", predicate.PropertyName);
            Assert.Equal("ee", predicate.Value);
            Assert.False(predicate.Not);
            Assert.Equal(Operator.Like, predicate.Operator);
        }

        PredicateGroup GetGroupFromQuery(IQueryBuilder<TestEntity> queryBuilder)
        {
            return (PredicateGroup)typeof(QueryBuilder<TestEntity>).GetField("_predicateGroup", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(queryBuilder);
        }
    }
}
