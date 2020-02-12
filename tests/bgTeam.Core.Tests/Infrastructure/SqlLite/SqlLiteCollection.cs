using Xunit;

namespace bgTeam.Core.Tests.Infrastructure
{
    [CollectionDefinition("SqlLiteCollection")]
    public class SqlLiteCollection : IClassFixture<SqlLiteFixture>
    {

    }
}
