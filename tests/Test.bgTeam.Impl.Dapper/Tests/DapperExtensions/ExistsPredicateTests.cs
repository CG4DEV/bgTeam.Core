using DapperExtensions;
using System;
using System.Collections.Generic;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.DapperExtensions
{
    public class ExistsPredicateTests
    {
        [Fact]
        public void GetSql()
        {
            var predicate = new ExistsPredicate<TestEntity>()
            {
                Predicate = new FieldPredicate<TestEntity>()
                { 
                    PropertyName = "Id",
                    Value = 2,
                    Operator = Operator.Eq,
                }
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(EXISTS (SELECT 1 FROM \"TestEntity\" WHERE (Id = @Id_0)))", sql);
            Assert.Equal(2, dictionary["@Id_0"]);
        }

        [Fact]
        public void GetSqlWhenValueIsNull()
        {
            var predicate = new ExistsPredicate<TestEntity>()
            {
                Predicate = new FieldPredicate<TestEntity>()
                {
                    PropertyName = "Id",
                    Value = 2,
                    Operator = Operator.Eq,
                },
                Not = true,
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(NOT EXISTS (SELECT 1 FROM \"TestEntity\" WHERE (Id = @Id_0)))", sql);
            Assert.Equal(2, dictionary["@Id_0"]);
        }
    }
}
