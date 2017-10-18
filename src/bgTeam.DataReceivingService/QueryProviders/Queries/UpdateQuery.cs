namespace bgTeam.DataReceivingService.QueryProviders.Queries
{
    using bgTeam.DataAccess;
    using bgTeam.DataReceivingService.QueryProviders;
    using bgTeam.ProcessMessages;
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UpdateQuery : AbstractQuery, IQuery
    {
        public UpdateQuery(IConnectionFactory factory, EntityMap map)
            : base (factory, map)
        {
        }

        public override async Task ExecuteAsync()
        {
            var sql = $"UPDATE {_map.TypeName} SET {GetSetColums()}, TimeStamp=GETDATE() WHERE {_map.KeyName} = @{_map.KeyName}";
            var dbArgs = GetArgs();

            using (var connection = _factory.Create())
            {
                await connection.ExecuteAsync(sql, dbArgs, commandTimeout: COMMAND_TIMEOUT);
            }
        }

        private DynamicParameters GetArgs()
        {
            var dbArgs = new DynamicParameters();
            foreach (var propertyName in OrderedColumns)
            {
                dbArgs.Add(propertyName, _map.Properties[propertyName]);
            }

            return dbArgs;
        }

        private string GetSetColums()
        {
            IEnumerable<string> setList = OrderedColumns
                .Except(new[] { _map.KeyName })
                .Select(x => $"{x} = @{x}");
            return string.Join(",", setList);
        }
    }
}
