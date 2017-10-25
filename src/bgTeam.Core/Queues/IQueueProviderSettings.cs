namespace bgTeam.Queues
{
    public interface IQueueProviderSettings
    {
        string Host { get; set; }

        int Port { get; set; }

        string VirtualHost { get; set; }

        string Login { get; set; }

        string Password { get; set; }
    }
}
