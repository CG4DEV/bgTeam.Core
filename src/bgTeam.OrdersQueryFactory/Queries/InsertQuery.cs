namespace bgTeam.OrdersQueryFactory.Queries
{
    using System;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using System.Linq;

    public class InsertQuery : AbstractDislocQuery
    {
        public InsertQuery(IConnectionFactory factory, EntityMap map) : base(factory, map)
        {
        }

        public override async Task ExecuteAsync()
        {
            var sql = $"INSERT INTO {_map.TypeName} ({GetColums()}) VALUES({GetValues()})";
            Dapper.DynamicParameters dbArgs;
            if (!_map.KeyName.Equals("IrsGuid"))
            {
                dbArgs = GetArgs();
                await ExecuteQuery(sql, dbArgs);
            }
        }

        private string GetValues()
        {
            return string.Join(",", OrderedColumns.Select(x => "@" + x));
        }

        private string GetColums()
        {
            return string.Join(",", OrderedColumns);
        }
    }
}
