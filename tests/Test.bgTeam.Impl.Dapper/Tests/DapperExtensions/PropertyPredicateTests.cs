using DapperExtensions;
using System.Collections.Generic;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.DapperExtensions
{
    public class PropertyPredicateTests
    {
        [Fact]
        public void GetSqlWithNotPredicate()
        {
            var predicate = new PropertyPredicate<TestEntity, TestEntity>()
            {
                Not = true,
                PropertyName = "Id",
                PropertyName2 = "Name",
                Operator = Operator.Eq
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(Id <> Name)", sql);
        }

        [Fact]
        public void GetSql()
        {
            var predicate = new PropertyPredicate<TestEntity, TestEntity>()
            {
                PropertyName = "Id",
                PropertyName2 = "Name",
                Operator = Operator.Eq
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(Id = Name)", sql);
        }
    }
}
