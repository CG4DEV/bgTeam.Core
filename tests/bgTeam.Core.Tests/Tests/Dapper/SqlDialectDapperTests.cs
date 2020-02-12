using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Dapper;
using DapperExtensions;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Dapper
{
    [Collection("SqlLiteCollection")]
    public class SqlDialectDapperTests
    {
        [Fact]
        public void MsSqlDialect()
        {
            var dapper = new SqlDialectDapper();
            dapper.Init(SqlDialectEnum.MsSql);
            var sql = dapper.GeneratePagingSql("select * from TestEntity where name = @Name", 2, 10, new { Name = "12" });
            Assert.StartsWith("SELECT TOP(10)", sql.Sql);
        }

        [Fact]
        public void MySqlDialect()
        {
            var dapper = new SqlDialectDapper();
            dapper.Init(SqlDialectEnum.MySql);
            var sql = dapper.GeneratePagingSql("select * from TestEntity where name = @Name", 2, 10, new { Name = "12" });
            Assert.Equal("select * from TestEntity where name = 12 LIMIT 20, 10", sql.ToString());
        }

        [Fact]
        public void OracleDialect()
        {
            var dapper = new SqlDialectDapper();
            dapper.Init(SqlDialectEnum.Oracle);
            var sql = dapper.GeneratePagingSql("select * from TestEntity where name = @Name", 2, 10, new { Name = "12" });
            Assert.StartsWith("SELECT * FROM (", sql.ToString());
        }

        [Fact]
        public void PostgreSqlDialect()
        {
            var dapper = new SqlDialectDapper();
            dapper.Init(SqlDialectEnum.PostgreSql);
            var sql = dapper.GeneratePagingSql("select * from TestEntity where name = @Name", 2, 10, new { Name = "12" });
            Assert.Equal("select * from TestEntity where name = 12 LIMIT 10 OFFSET 20", sql.ToString());
        }

        [Fact]
        public void SqliteDialect()
        {
            var dapper = new SqlDialectDapper();
            dapper.Init(SqlDialectEnum.Sqlite);
            var sql = dapper.GeneratePagingSql("select * from TestEntity where name = @Name", 2, 10, new { Name = "12" });
            Assert.Equal("select * from TestEntity where name = 12 LIMIT 20, 10", sql.ToString());
        }

        [Fact]
        public void QueryShouldNotBeNullForGeneratingSql()
        {
            var dapper = new SqlDialectDapper();
            dapper.Init(SqlDialectEnum.Sqlite);
            Assert.Throws<ArgumentNullException>("query", () =>
            {
                dapper.GeneratePagingSql(null, 2, 1, null);
            });
        }

        [Fact]
        public void SqlDialectShouldNotBeNullForGeneratingSql()
        {
            var dapper = new SqlDialectDapper();
            DapperHelper.SqlDialect = null;
            Assert.Throws<InvalidOperationException>(() =>
            {
                dapper.GeneratePagingSql("select * from TestEntity", 2, 1, null);
            });
        }
    }
}
