using DapperExtensions.Mapper.Sql;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.Mapper
{
    public class SqlServerDialectTests
    {
        [Fact]
        public void OpenQuote()
        {
            var dialect = new SqlServerDialect();
            Assert.Equal('[', dialect.OpenQuote);
        }

        [Fact]
        public void CloseQuote()
        {
            var dialect = new SqlServerDialect();
            Assert.Equal(']', dialect.CloseQuote);
        }

        [Fact]
        public void GetIdentitySql()
        {
            var dialect = new SqlServerDialect();
            Assert.Equal("SELECT CAST(SCOPE_IDENTITY()  AS BIGINT) AS [Id]", dialect.GetIdentitySql("table"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetSetSqlShouldThrowsExceptionIfSqlIsNullOrEmpty(string sql)
        {
            var dialect = new SqlServerDialect();
            Assert.Throws<ArgumentNullException>("sql", () =>
            {
                dialect.GetSetSql(sql, 1, 1, null);
            });
        }

        [Fact]
        public void GetSetSqlShouldThrowsExceptionIfParamsIsNullEmpty()
        {
            var dialect = new SqlServerDialect();
            Assert.Throws<ArgumentNullException>("parameters", () =>
            {
                dialect.GetSetSql("select * from TestEntity", 1, 1, null);
            });
        }

        [Fact]
        public void GetPagingSql()
        {
            var dialect = new SqlServerDialect();
            var dictionary = new Dictionary<string, object>();
            var query = dialect.GetPagingSql("select distinct name as first_name from TestEntity order by id where id > 2", 2, 10, dictionary);

            Assert.Equal("SELECT TOP(10) [_proj].[first_name] FROM (select distinct ROW_NUMBER() OVER(ORDER BY id) AS [_row_number], name as first_name from TestEntity where id > 2) [_proj] " +
                "WHERE [_proj].[_row_number] >= @_pageStartRow ORDER BY [_proj].[_row_number]", query);

            Assert.Equal(21, dictionary["@_pageStartRow"]);

            dictionary = new Dictionary<string, object>();
            query = dialect.GetPagingSql("select id, name from TestEntity order by id", 2, 10, dictionary);
            Assert.Equal("SELECT TOP(10) [_proj].[id], [_proj].[name] FROM (select ROW_NUMBER() OVER(ORDER BY id) AS [_row_number], id, name from TestEntity) [_proj] " +
                "WHERE [_proj].[_row_number] >= @_pageStartRow ORDER BY [_proj].[_row_number]", query);
        }

        [Fact]
        public void GetSqlShouldThrowsExceptionIfQueryIsNotSelectStatement()
        {
            var dialect = new SqlServerDialect();
            Assert.Throws<ArgumentException>("sql", () =>
            {
                dialect.GetSetSql("update TestEntity set name = 'Alex'", 1, 1, new Dictionary<string, object>());
            });
        }
    }
}
