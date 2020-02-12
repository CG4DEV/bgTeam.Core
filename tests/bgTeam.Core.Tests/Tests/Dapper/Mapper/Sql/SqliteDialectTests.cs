using DapperExtensions.Mapper.Sql;
using System;
using System.Collections.Generic;
using Xunit;

namespace bgTeam.Core.Tests.Dapper.Mapper.Sql
{
    public class SqliteDialectTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetSetSqlShouldThrowsExceptionIfSqlIsNullOrEmpty(string sql)
        {
            var dialect = new SqliteDialect();
            Assert.Throws<ArgumentNullException>("sql", () =>
            {
                dialect.GetSetSql(sql, 1, 1, null);
            });
        }

        [Fact]
        public void GetSetSqlShouldThrowsExceptionIfParamsIsNullEmpty()
        {
            var dialect = new SqliteDialect();
            Assert.Throws<ArgumentNullException>("parameters", () =>
            {
                dialect.GetSetSql("select * from TestEntity", 1, 1, null);
            });
        }

        [Fact]
        public void GetSetSql()
        {
            var dialect = new SqliteDialect();
            var dictionary = new Dictionary<string, object>();
            Assert.Equal("select * from TestEntity LIMIT @Offset, @Count", dialect.GetSetSql("select * from TestEntity", 1, 2, dictionary));
            Assert.Equal(1, dictionary["@Offset"]);
            Assert.Equal(2, dictionary["@Count"]);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetColumnNameShouldThrowsExceptionIfSqlIsNullOrEmpty(string sql)
        {
            var dialect = new SqliteDialect();
            Assert.Throws<ArgumentNullException>("columnName", () =>
            {
                dialect.GetColumnName(string.Empty, sql, string.Empty);
            });
        }

        [Fact]
        public void GetColumnName()
        {
            var dialect = new SqliteDialect();
            Assert.Equal("name AS \"first_name\"", dialect.GetColumnName("pr", "name", "first_name"));
        }
    }
}
