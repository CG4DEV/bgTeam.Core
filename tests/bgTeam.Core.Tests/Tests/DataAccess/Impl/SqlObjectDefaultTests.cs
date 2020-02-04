using bgTeam.DataAccess.Impl;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl
{
    public class SqlObjectDefaultTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DependencySql(string sql)
        {
            Assert.Throws<ArgumentNullException>("sql", () =>
            {
                new SqlObjectDefault(sql);
            });
        }

        [Fact]
        public void GeneratingStringForDebugging()
        {
            var sql = new SqlObjectDefault(@"select * from users where name = @Name and age > @Age", new System.Collections.Generic.Dictionary<string, object>
            {
                { "@Name", "John" },
                { "@Age", "15" }
            } );
            Assert.Equal("select * from users where name = John and age > 15", sql.ToString());
        }
    }
}
