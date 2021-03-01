using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bgTeam.Impl.Kafka;

namespace KafkaSingleThread
{
    class Program
    {
        static void Main(string[] args)
        {
            // inherit IKafkaSettings
            var settings = new KafkaSettings
            {
                // See more configs: https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md
                Config = new Dictionary<string, string>
                {
                    { "group.id", "EdgeBetDetector-test" },
                    { "bootstrap.servers", "192.168.30.173:9092,192.168.30.221:9092,192.168.30.222:9092" },
                },
            };

            var consumer = new KafkaQueueWatcher<KafkaMessage>(settings);

            consumer.Subscribe += async (msg) =>
            {
                await Task.Yield();

                var kafkaMessage = (KafkaMessage)msg;

                Console.WriteLine($"New message: {kafkaMessage.Partition}, {kafkaMessage.Offset}");
            };

            consumer.KafkaLogs += (sender, log) => Console.WriteLine($"Log: " + log);
            consumer.Error += (sender, err) => Console.WriteLine($"Error: {err.Exception?.Message}");

            consumer.StartWatch("BetAggregator_Bets-Prod-B2B");

            Console.ReadLine();

            consumer.Close();
        }
    }
}
