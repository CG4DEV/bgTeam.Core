using bgTeam.Core.Tests.Infrastructure;
using DapperExtensions;
using System.Collections.Generic;
using Xunit;

namespace bgTeam.Core.Tests.Dapper.DapperExtensions
{
    public class BetweenPredicateTests
    {
        [Fact]
        public void GetSqlWithNotPredicate()
        {
            var predicate = new BetweenPredicate<TestEntity>()
            {
                Value = new BetweenValues()
                {
                    Value1 = 10,
                    Value2 = 20,
                },
                Not = true,
                PropertyName = "Id"
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(Id NOT BETWEEN @Id_0 AND @Id_1)", sql);
            Assert.Equal(10, dictionary["@Id_0"]);
            Assert.Equal(20, dictionary["@Id_1"]);
        }

        [Fact]
        public void GetSql()
        {
            var predicate = new BetweenPredicate<TestEntity>()
            {
                Value = new BetweenValues()
                {
                    Value1 = 10,
                    Value2 = 20,
                },
                Not = false,
                PropertyName = "Id"
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(Id BETWEEN @Id_0 AND @Id_1)", sql);
            Assert.Equal(10, dictionary["@Id_0"]);
            Assert.Equal(20, dictionary["@Id_1"]);
        }
    }
}
