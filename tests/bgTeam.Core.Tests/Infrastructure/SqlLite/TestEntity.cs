using bgTeam.DataAccess.Impl.Dapper;

namespace bgTeam.Core.Tests.Infrastructure
{
    [TableName("TestEntity")]
    public class TestEntity
    {
        [Identity]
        [PrymaryKey]
        public int? Id { get; set; }

        public string Name { get; set; }
    }
}
