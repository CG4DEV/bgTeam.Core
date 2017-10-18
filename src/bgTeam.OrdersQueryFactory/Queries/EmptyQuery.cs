namespace bgTeam.OrdersQueryFactory.Queries
{
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using System.Threading.Tasks;
    using System;

    public class EmptyQuery : IQuery
    {
        public void Execute()
        {
            
        }

        public Task ExecuteAsync()
        {
            return Task.Run(() => { });
        }
    }
}
