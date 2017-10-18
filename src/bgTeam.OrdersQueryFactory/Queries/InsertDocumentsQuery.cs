namespace bgTeam.OrdersQueryFactory.Queries
{
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using Dapper;
    using System.Linq;
    using System.Threading.Tasks;

    public class InsertDocumentsQuery : AbstractDocumentsQuery, IQuery
    {
        public InsertDocumentsQuery(IRepository repository, IConnectionFactory factory, EntityMap map)
            : base (repository, factory, map)
        {
        }

        public override async Task ExecuteAsync()
        {
            var sql = $"INSERT INTO {_map.TypeName} ({GetColums()}) VALUES({GetValues()})";
            var dbArgs = GetArgs();

            using (var connection = _factory.Create())
            {
                await connection.ExecuteAsync(sql, dbArgs, commandTimeout: COMMAND_TIMEOUT);
                await ProcessOwnerAsync(connection);
                await ProcessConnectionAsync(connection);
            }
        }

        private DynamicParameters GetArgs()
        {
            var dbArgs = new DynamicParameters();
            foreach (var propertyName in DocumentsColumns)
            {
                dbArgs.Add(propertyName, _map.Properties[propertyName]);
            }

            return dbArgs;
        }

        private string GetValues()
        {
            return string.Join(",", DocumentsColumns.Select(x => "@" + x));
        }

        private string GetColums()
        {
            return string.Join(",", DocumentsColumns);
        }
    }
}
