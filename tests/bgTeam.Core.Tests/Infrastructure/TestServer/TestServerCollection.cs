using Xunit;

namespace bgTeam.Core.Tests.Infrastructure.TestServer
{
    [CollectionDefinition("TestServerCollection")]
    public class TestServerCollection : IClassFixture<TestServerFixture>
    {
    }
}
