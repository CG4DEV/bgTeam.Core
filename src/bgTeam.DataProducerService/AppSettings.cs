namespace bgTeam.DataProducerService
{
    using bgTeam.Core;
    using bgTeam.DataAccess;
    using bgTeam.Queues;

    internal class AppSettings : IConnectionSetting, IQueueProviderSettings
    {
        private readonly IAppConfiguration _appConfiguration;

        public string ConnectionString { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string VirtualHost { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public AppSettings(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;

            ConnectionString = appConfiguration.GetConnectionString("PRODUCERDB");

            Host = appConfiguration["RubbitMQ:Host"];
            Port = int.Parse(appConfiguration["RubbitMQ:Port"]);
            VirtualHost = appConfiguration["RubbitMQ:VirtualHost"];
            Login = appConfiguration["RubbitMQ:Login"];
            Password = appConfiguration["RubbitMQ:Password"];
        }
    }
}
