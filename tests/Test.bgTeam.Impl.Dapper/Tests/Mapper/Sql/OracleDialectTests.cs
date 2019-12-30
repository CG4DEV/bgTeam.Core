using DapperExtensions.Mapper.Sql;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.Mapper
{
    public class OracleDialectTests
    {
        [Fact]
        public void SupportsMultipleStatements()
        {
            var dialect = new OracleDialect();
            Assert.False(dialect.SupportsMultipleStatements);
        }

        [Fact]
        public void ParameterPrefix()
        {
            var dialect = new OracleDialect();
            Assert.Equal(':', dialect.ParameterPrefix);
        }

        [Fact]
        public void OpenQuote()
        {
            var dialect = new OracleDialect();
            Assert.Equal('"', dialect.OpenQuote);
        }

        [Fact]
        public void CloseQuote()
        {
            var dialect = new OracleDialect();
            Assert.Equal('"', dialect.CloseQuote);
        }

        [Fact]
        public void GetIdentitySql()
        {
            var dialect = new OracleDialect();
            Assert.Throws<NotImplementedException>(() =>
            {
                dialect.GetIdentitySql("table");
            });
        }

        [Fact]
        public void GetPagingSql()
        {
            var dialect = new OracleDialect();
            var dictionary = new Dictionary<string, object>();
            var query = dialect.GetPagingSql("select * from TestEntity", 2, 10, dictionary);
            Assert.Contains("ROWNUM RNUM FROM", query);
            Assert.Equal(30, dictionary[":topLimit"]);
            Assert.Equal(20, dictionary[":toSkip"]);
        }

        [Fact]
        public void GetSetSql()
        {
            var dialect = new OracleDialect();
            var dictionary = new Dictionary<string, object>();
            var query = dialect.GetSetSql("select * from TestEntity", 2, 10, dictionary);
            Assert.Contains("ROWNUM RNUM FROM", query);
            Assert.Equal(12, dictionary[":topLimit"]);
            Assert.Equal(2, dictionary[":toSkip"]);
        }

        [Fact]
        public void QuoteString()
        {
            var dialect = new OracleDialect();
            Assert.Equal("\"select * from TestEntity\"", dialect.QuoteString("`select * from TestEntity`"));
            Assert.Equal("SELECT * FROM TESTENTITY", dialect.QuoteString("select * from TestEntity"));
        }
    }
}
