using bgTeam.Impl;
using bgTeam.Web;
using bgTeam.Web.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new AppLoggerDefault();
            var url = "http://booking.trcont.ru/RisApi/";
            var client = new WebClient(logger, url)
            {
                ConnectionLeaseTimeout = 0,
                //ConnectionsLimit = 10,
                MaxIdleTime = 120000
            };

            Console.WriteLine($"Количество одновременных соединений {client.ConnectionsLimit}");
            client.ConnectionsLimit = 10;

            var tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(client.PostAsync<dynamic>("Info/GetOrderInfoById", new { Id = 60081373 }));
            }

            Task.WaitAll(tasks.ToArray());

            var results = tasks.Select(x => ((Task<dynamic>)x).Result).ToArray();

            Console.ReadKey();
        }
    }
}
