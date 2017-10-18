namespace bgTeam.OrdersQueryFactory.Queries
{
    using System;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using System.Collections.Generic;
    using System.Linq;

    public class UpdateQuery : AbstractDislocQuery
    {
        public UpdateQuery(IConnectionFactory factory, EntityMap map) : base(factory, map)
        {
        }

        public override async Task ExecuteAsync()
        {
            var sql = $"UPDATE {_map.TypeName} SET {GetSetColums()}, TimeStamp=GETDATE() WHERE {_map.KeyName} = @{_map.KeyName}";
            var dbArgs = GetArgs();
            await ExecuteQuery(sql, dbArgs);
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
