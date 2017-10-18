namespace bgTeam.OrdersQueryFactory.Queries
{
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;

    public abstract class AbstractDislocQuery : IQuery
    {
        protected const int COMMAND_TIMEOUT = 300;
        protected readonly IConnectionFactory _factory;
        protected readonly EntityMap _map;

        public IEnumerable<string> OrderedColumns => _map.PropertyNames.OrderBy(x => x);

        public AbstractDislocQuery(IConnectionFactory factory, EntityMap map)
        {
            _factory = factory;
            _map = map;
        }

        public virtual void Execute()
        {
            ExecuteAsync().Wait();
        }

        public abstract Task ExecuteAsync();

        protected async Task ExecuteQuery(string sql, object sqlArgs)
        {
            using (var connection = _factory.Create())
            {
                await connection.ExecuteAsync(sql, sqlArgs, commandTimeout: COMMAND_TIMEOUT);
            }
        }

        protected DynamicParameters GetArgs(params string[] withoutColumns)
        {
            var dbArgs = new DynamicParameters();
            foreach (var propertyName in _map.PropertyNames.Except(withoutColumns).OrderBy(x => x))
            {
                dbArgs.Add(propertyName, _map.Properties[propertyName]);
            }

            return dbArgs;
        }
    }
}
