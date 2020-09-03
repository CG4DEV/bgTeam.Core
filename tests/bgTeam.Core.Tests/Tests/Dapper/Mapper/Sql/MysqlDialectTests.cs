using DapperExtensions.Mapper.Sql;
using System;
using System.Collections.Generic;
using Xunit;

namespace bgTeam.Core.Tests.Dapper.Mapper.Sql
{
    public class MysqlDialectTests
    {

        [Fact]
        public void OpenQuote()
        {
            var dialect = new MySqlDialect();
            Assert.Equal('`', dialect.OpenQuote);
        }

        [Fact]
        public void CloseQuote()
        {
            var dialect = new MySqlDialect();
            Assert.Equal('`', dialect.CloseQuote);
        }

        [Fact]
        public void GetIdentitySql()
        {
            var dialect = new MySqlDialect();
            Assert.Equal("SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS ID", dialect.GetIdentitySql("table"));
        }

        [Fact]
        public void GetPagingSql()
        {
            var dialect = new MySqlDialect();
            var dictionary = new Dictionary<string, object>();
            Assert.Equal("select * from TestEntity LIMIT @firstResult, @maxResults", dialect.GetPagingSql("select * from TestEntity", 2, 10, dictionary));
            Assert.Equal(20, dictionary["@firstResult"]);
            Assert.Equal(10, dictionary["@maxResults"]);
        }

        [Fact]
        public void EmptyExpression()
        {
            var dialect = new MySqlDialect();
            Assert.Equal("1=1", dialect.EmptyExpression);
        }

        [Fact]
        public void IsQuoted()
        {
            var dialect = new MySqlDialect();
            Assert.True(dialect.IsQuoted("`value`"));
            Assert.False(dialect.IsQuoted("`value"));
        }

        [Fact]
        public void QuoteString()
        {
            var dialect = new MySqlDialect();
            Assert.Equal("`value`", dialect.QuoteString("value"));
            Assert.Equal("`value`", dialect.QuoteString("`value`"));
        }

        [Fact]
        public void UnQuoteString()
        {
            var dialect = new MySqlDialect();
            Assert.Equal("value", dialect.UnQuoteString("`value`"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetTableNameShouldThrowsExceptionIfTableNameIsNullOrEmpty(string tableName)
        {
            var dialect = new MySqlDialect();
            Assert.Throws<ArgumentNullException>("tableName", () =>
            {
                dialect.GetTableName("scheme", tableName, "t");
            });
        }

        [Fact]
        public void GetTableName()
        {
            var dialect = new MySqlDialect();
            Assert.Equal("`schema`.`table` AS `t`", dialect.GetTableName("schema", "table", "t"));
            Assert.Equal("`table`", dialect.GetTableName(null, "table", null));
        }
    }
}
