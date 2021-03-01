using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using bgTeam.Impl.Kafka;

namespace KafkaMultiThread
{
    class Program
    {
        static void Main(string[] args)
        {
            // inherit IMultithreadKafkaSettings
            var settings = new MultithreadKafkaSettings
            {
                ThreadCount = 16,
                BoundedCapacity = 128,
                CommitIntervalMs = 200,

                // See more configs: https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md
                Config = new Dictionary<string, string>
                {
                    { "group.id", "SingleThreadWatcher" },
                    { "bootstrap.servers", "localhost:9092" },
                },
            };

            var consumer = new MultithreadKafkaQueueWatcher<KafkaMessage>(settings);

            consumer.Subscribe += async (msg) =>
            {
                await Task.Yield();
                var kafkaMessage = (KafkaMessage)msg;

                Console.WriteLine($"New message: {kafkaMessage.Partition}, {kafkaMessage.Offset}, {Thread.CurrentThread.ManagedThreadId}");
            };

            consumer.KafkaLogs += (sender, log) => Console.WriteLine($"Log: " + log);
            consumer.Error += (sender, err) => Console.WriteLine($"Error: {err.Exception?.Message}");

            consumer.StartWatch("LogTopic");

            Console.ReadLine();

            consumer.Close();

            Thread.Sleep(1000);
        }
    }
}
