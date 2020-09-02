using bgTeam.Core.Impl;
using bgTeam.DataAccess;
using bgTeam.Queues;

namespace bgTeam.StoryRunnerScheduler.Example
{
    internal class AppSettings : IConnectionSetting, IQueueProviderSettings
    {
        public AppSettings(AppConfigurationDefault config)
        {
        }

        public string ConnectionString { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Host { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Port { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string VirtualHost { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Login { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Password { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
