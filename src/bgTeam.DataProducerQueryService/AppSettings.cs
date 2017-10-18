namespace bgTeam.DataProducerQueryService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.Queues;
    using bgTeam.Core;

    internal class AppSettings : IConnectionSetting, IQueueProviderSettings
    {
        private IAppConfiguration _appConfiguration;

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
