using bgTeam.DataAccess.Impl.Dapper;

namespace bgTeam.Core.Tests.Infrastructure
{
    [TableName("TestEntity")]
    public class TestEntity
    {
        [Identity]
        [PrimaryKey]
        public int? Id { get; set; }

        public string Name { get; set; }
    }

    [TableName("CompositeKeyEntity")]
    public class CompositeKeyEntity
    {
        [PrimaryKey]
        public int Key1 { get; set; }

        [PrimaryKey]
        public int Key2 { get; set; }

        public string Name { get; set; }
    }
}