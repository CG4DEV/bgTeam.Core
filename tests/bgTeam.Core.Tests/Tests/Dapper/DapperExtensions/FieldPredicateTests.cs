using bgTeam.Core.Tests.Infrastructure;
using DapperExtensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace bgTeam.Core.Tests.Dapper.DapperExtensions
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

        [Theory]
        [InlineData(true, Operator.Gt, "<=")]
        [InlineData(false, Operator.Gt, ">")]
        [InlineData(true, Operator.Ge, "<")]
        [InlineData(false, Operator.Ge, ">=")]
        [InlineData(true, Operator.Lt, ">=")]
        [InlineData(false, Operator.Lt, "<")]
        [InlineData(true, Operator.Le, ">")]
        [InlineData(false, Operator.Le, "<=")]
        [InlineData(true, Operator.Like, "NOT LIKE")]
        [InlineData(false, Operator.Like, "LIKE")]
        [InlineData(true, Operator.Eq, "<>")]
        [InlineData(false, Operator.Eq, "=")]
        public void GeneratingOperatorStrings(bool not, Operator @operator, string excepted)
        {
            var predicate = new FieldPredicate<TestEntity>()
            {
                Value = 2,
                Operator = @operator,
                PropertyName = "Id",
                Not = not
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal($"(Id {excepted} @Id_0)", sql);
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
