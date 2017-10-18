using bgTeam.DataAccess;
using bgTeam.ProcessMessages;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgTeam.OrdersQueryFactory.Queries
{
    public class UpdateTeoByOrder : AbstractQuery, IQuery
    {
        public UpdateTeoByOrder(IConnectionFactory factory, EntityMap map)
            : base (factory, map)
        {
        }

        public override async Task ExecuteAsync()
        {
            var columnsToUpdate = new string[] 
            {
                "IrsGuid",
                "ContrRelationsTypeId",
                "SendTypeId",
                "CustomType"
            };
            var sql = $"UPDATE {_map.TypeName} SET {GetSetColums(columnsToUpdate)}, TimeStamp=GETDATE() WHERE {_map.KeyName} = @{_map.KeyName}";
            var dbArgs = GetArgs(columnsToUpdate);
            dbArgs.Add(_map.KeyName, _map.KeyValue);

            using (var connection = _factory.Create())
            {
                await connection.ExecuteAsync(sql, dbArgs, commandTimeout: COMMAND_TIMEOUT);
            }
        }

        private DynamicParameters GetArgs(params string[] columns)
        {
            var dbArgs = new DynamicParameters();
            foreach (var propertyName in columns)
            {
                dbArgs.Add(propertyName, _map.Properties[propertyName]);
            }

            return dbArgs;
        }

        private string GetSetColums(params string[] columns)
        {
            IEnumerable<string> setList = columns
                .Except(new[] { _map.KeyName })
                .Select(x => $"{x} = @{x}");
            return string.Join(",", setList);
        }
    }
}
