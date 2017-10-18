namespace bgTeam.DataReceivingService.QueryProviders.Queries
{
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class AbstractQuery : IQuery
    {
        protected const int COMMAND_TIMEOUT = 300;
        protected readonly IConnectionFactory _factory;
        protected readonly EntityMap _map;

        public IEnumerable<string> OrderedColumns => _map.PropertyNames.OrderBy(x => x);

        public AbstractQuery(IConnectionFactory factory, EntityMap map)
        {
            _factory = factory;
            _map = map;
        }

        public virtual void Execute()
        {
            ExecuteAsync().Wait();
        }

        public abstract Task ExecuteAsync();
    }
}
