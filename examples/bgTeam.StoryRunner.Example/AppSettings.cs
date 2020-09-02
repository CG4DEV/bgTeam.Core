using bgTeam.Core.Impl;
using bgTeam.Queues;

namespace bgTeam.StoryRunner.Example
{
    internal class AppSettings : IQueueProviderSettings
    {
        public AppSettings(AppConfigurationDefault config)
        {
        }

        public string Host { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Port { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string VirtualHost { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Login { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Password { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
