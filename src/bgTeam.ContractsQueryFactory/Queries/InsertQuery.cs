namespace bgTeam.ContractsQueryFactory.Queries
{
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using Dapper;
    using System.Linq;
    using System.Threading.Tasks;

    public class InsertQuery : AbstractQuery, IQuery
    {
        public InsertQuery(IConnectionFactory factory, EntityMap map)
            : base (factory, map)
        {
        }

        public override async Task ExecuteAsync()
        {
            var sql = $"INSERT INTO {_map.TypeName} ({GetColums()}) VALUES({GetValues()})";
            var dbArgs = GetArgs();

            using (var connection = _factory.Create())
            {
                await connection.ExecuteAsync(sql, dbArgs, commandTimeout: COMMAND_TIMEOUT);
            }
        }

        private DynamicParameters GetArgs()
        {
            var dbArgs = new DynamicParameters();
            foreach (var propertyName in _map.PropertyNames.OrderBy(x => x))
            {
                dbArgs.Add(propertyName, _map.Properties[propertyName]);
            }

            return dbArgs;
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
