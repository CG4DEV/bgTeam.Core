using DapperExtensions.Mapper.Sql;
using System;
using System.Collections.Generic;
using Xunit;

namespace bgTeam.Core.Tests.Dapper.Mapper.Sql
{
    public class SqlCeDialectTests
    {

        [Fact]
        public void OpenQuote()
        {
            var dialect = new SqlCeDialect();
            Assert.Equal('[', dialect.OpenQuote);
        }

        [Fact]
        public void CloseQuote()
        {
            var dialect = new SqlCeDialect();
            Assert.Equal(']', dialect.CloseQuote);
        }

        [Fact]
        public void SupportsMultipleStatements()
        {
            var dialect = new SqlCeDialect();
            Assert.False(dialect.SupportsMultipleStatements);
        }

        [Fact]
        public void GetTableName()
        {
            var dialect = new SqlCeDialect();
            Assert.Equal("[scheme_table] AS [t]", dialect.GetTableName("scheme", "table", "t"));
            Assert.Equal("[table]", dialect.GetTableName(null, "table", null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetTableNameShouldThrowsExceptionIfTableNameIsNullOrEmpty(string tableName)
        {
            var dialect = new SqlCeDialect();
            Assert.Throws<ArgumentNullException>("tableName", () =>
            {
                dialect.GetTableName("scheme", tableName, "t");
            });
        }

        [Fact]
        public void GetIdentitySql()
        {
            var dialect = new SqlCeDialect();
            Assert.Equal("SELECT CAST(@@IDENTITY AS BIGINT) AS [Id]", dialect.GetIdentitySql("table"));
        }

        [Fact]
        public void GetPagingSql()
        {
            var dialect = new SqlCeDialect();
            var dictionary = new Dictionary<string, object>();
            var query = dialect.GetPagingSql("select distinct name as first_name from TestEntity order by id where id > 2", 2, 10, dictionary);

            Assert.Equal("select distinct name as first_name from TestEntity order by id where id > 2 OFFSET @firstResult ROWS FETCH NEXT @maxResults ROWS ONLY", query);
            Assert.Equal(20, dictionary["@firstResult"]);
            Assert.Equal(10, dictionary["@maxResults"]);
        }
    }
}
