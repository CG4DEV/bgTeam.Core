namespace Trcont.Cud.Infrastructure.Impl
{
    using bgTeam.Infrastructure.DataAccess;

    public class StInfoRepository : RepositoryDapper, IStInfoRepository
    {
        public StInfoRepository(IStInfoConnectionFactory connFactory)
            : base(connFactory)
        {
        }
    }
}
