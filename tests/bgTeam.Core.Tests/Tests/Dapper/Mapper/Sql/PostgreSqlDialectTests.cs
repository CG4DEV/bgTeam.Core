using DapperExtensions.Mapper.Sql;
using System;
using System.Collections.Generic;
using Xunit;

namespace bgTeam.Core.Tests.Dapper.Mapper.Sql
{
    public class PostgreSqlDialectTests
    {
        [Fact]
        public void GetIdentitySql()
        {
            var dialect = new PostgreSqlDialect();
            Assert.Equal("SELECT LASTVAL() AS Id", dialect.GetIdentitySql("table"));
        }

        [Fact]
        public void GetPagingSql()
        {
            var dialect = new PostgreSqlDialect();
            var dictionary = new Dictionary<string, object>();
            Assert.Equal("select * from TestEntity LIMIT @maxResults OFFSET @pageStartRowNbr", dialect.GetPagingSql("select * from TestEntity", 1, 2, dictionary));
            Assert.Equal(2, dictionary["@maxResults"]);
            Assert.Equal(2, dictionary["@pageStartRowNbr"]);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetColumnNameShouldThrowsExceptionIfSqlIsNullOrEmpty(string sql)
        {
            var dialect = new PostgreSqlDialect();
            Assert.Throws<ArgumentNullException>("columnName", () =>
            {
                dialect.GetColumnName(string.Empty, sql, string.Empty);
            });
        }

        [Fact]
        public void GetColumnName()
        {
            var dialect = new PostgreSqlDialect();
            Assert.Equal("\"name\" AS \"first_name\"", dialect.GetColumnName("pr", "name", "first_name"));
        }
    }
}
