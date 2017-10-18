namespace bgTeam.OrdersQueryFactory.Queries
{
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using Dapper;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UpdateDocumentsQuery : AbstractDocumentsQuery, IQuery
    {
        public UpdateDocumentsQuery(IRepository repository, IConnectionFactory factory, EntityMap map)
            : base (repository, factory, map)
        {
        }

        public override async Task ExecuteAsync()
        {
            var sql = $"UPDATE {_map.TypeName} SET {GetSetColums()}, TimeStamp=GETDATE() WHERE {_map.KeyName} = @{_map.KeyName}";
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

        private string GetSetColums()
        {
            IEnumerable<string> setList = DocumentsColumns
                .Except(new[] { _map.KeyName })
                .Select(x => $"{x} = @{x}");
            return string.Join(",", setList);
        }
    }
}
