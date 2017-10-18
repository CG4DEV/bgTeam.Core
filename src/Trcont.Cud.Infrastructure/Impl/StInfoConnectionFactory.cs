namespace Trcont.Cud.Infrastructure.Impl
{
    using bgTeam;
    using bgTeam.Infrastructure.DataAccess;

    public class StInfoConnectionFactory : ConnectionFactoryMsSql, IStInfoConnectionFactory
    {
        public StInfoConnectionFactory(IAppLogger logger, IStInfoConnectionSetting settings)
            : base(logger, settings)
        {
        }

        public StInfoConnectionFactory(IAppLogger logger, string connectionString)
            : base(logger, connectionString)
        {
        }
    }
}
