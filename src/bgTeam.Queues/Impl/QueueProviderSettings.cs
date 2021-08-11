namespace bgTeam.Queues
{
    public class QueueProviderSettings : IQueueProviderSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string VirtualHost { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool DispatchConsumersAsync { get; set; }

        public string ClientProvidedName { get; set; }
    }
}