using DapperExtensions;
using System;
using System.Collections.Generic;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.DapperExtensions
{
    public class FieldPredicateTests
    {
        [Fact]
        public void GetSql()
        {
            var predicate = new FieldPredicate<TestEntity>()
            {
                Value = 2,
                Operator = Operator.Gt,
                PropertyName = "Id"
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(Id > @Id_0)", sql);
            Assert.Equal(2, dictionary["@Id_0"]);
        }

        [Fact]
        public void GetSqlWhenValueIsNull()
        {
            var predicate = new FieldPredicate<TestEntity>()
            {
                Operator = Operator.Gt,
                PropertyName = "Id"
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(Id IS NULL)", sql);
        }

        [Fact]
        public void GetSqlWhenValueIsNullAndIsNotExpression()
        {
            var predicate = new FieldPredicate<TestEntity>()
            {
                Operator = Operator.Gt,
                PropertyName = "Id",
                Not = true
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(Id IS NOT NULL)", sql);
        }

        [Fact]
        public void GetSqlWhenValueIsEnumerable()
        {
            var predicate = new FieldPredicate<TestEntity>()
            {
                Value = new[] { 2, 5 },
                Operator = Operator.Eq,
                PropertyName = "Id"
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(Id IN (@Id_0, @Id_1))", sql);
            Assert.Equal(2, dictionary["@Id_0"]);
            Assert.Equal(5, dictionary["@Id_1"]);
        }

        [Fact]
        public void GetSqlWhenValueIsEnumerableAndIsNotExpression()
        {
            var predicate = new FieldPredicate<TestEntity>()
            {
                Value = new[] { 2, 5 },
                Operator = Operator.Eq,
                PropertyName = "Id",
                Not = true
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(Id NOT IN (@Id_0, @Id_1))", sql);
            Assert.Equal(2, dictionary["@Id_0"]);
            Assert.Equal(5, dictionary["@Id_1"]);
        }

        [Theory]
        [InlineData(Operator.Ge)]
        [InlineData(Operator.Gt)]
        [InlineData(Operator.Le)]
        [InlineData(Operator.Like)]
        [InlineData(Operator.Lt)]
        public void GetSqlWhenValueIsEnumerableAndOperatorIsNotEqShouldThrowsException(Operator op)
        {
            var predicate = new FieldPredicate<TestEntity>()
            {
                Value = new[] { 2, 5 },
                Operator = op,
                PropertyName = "Id"
            };

            Assert.Throws<ArgumentException>(() =>
            {
                predicate.GetSql(SqlHelper.GetSqlGenerator(), new Dictionary<string, object>());
            });
        }
    }
}
