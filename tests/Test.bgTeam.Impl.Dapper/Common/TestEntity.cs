using bgTeam.DataAccess.Impl.Dapper;

namespace Test.bgTeam.Impl.Dapper.Common
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
