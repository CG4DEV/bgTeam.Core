using Test.bgTeam.Impl.Dapper.Tests;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Common
{
    [CollectionDefinition("SqlLiteCollection")]
    public class SqlLiteCollection : IClassFixture<SqlLiteFixture>
    {

    }
}
