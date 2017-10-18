namespace bgTeam.Queues
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IQueueProviderSettings
    {
        string Host { get; set; }

        int Port { get; set; }

        string VirtualHost { get; set; }

        string Login { get; set; }

        string Password { get; set; }
    }

    public class QueueProviderSettings : IQueueProviderSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string VirtualHost { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
