namespace bgTeam.DataProducerQueryService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore.Common;
    using bgTeam.Exceptions.Args;
    using bgTeam.ProduceMessages;
    using bgTeam.Queues;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class Runner
    {
        private readonly IAppLogger _logger;
        private readonly IQueueWatcher<IQueueMessage> _queueWatcher;
        private readonly IQueueProvider _queueProvider;
        private readonly IQueueProvider _queueProviderErrors;
        private readonly ISenderEntity _responseProvider;
        private readonly ISenderEntity _responseNotifications;
        private readonly IRepositoryData _repositoryData;
        private IEnumerable<DictionaryConfig> _configurations;
        private readonly string _watchQueue;

        public Runner(
            IAppLogger logger,
            IQueueWatcher<IQueueMessage> queueWatcher,
            IRepositoryData repositoryData,
            IQueueProvider queueProvider,
            IQueueProvider queueProviderErrors,
            ISenderEntity responseProvider,
            ISenderEntity responseNotifications,
            string watchQueue)
        {
            _logger = logger;
            _queueWatcher = queueWatcher;
            _queueProvider = queueProvider;
            _queueProviderErrors = queueProviderErrors;
            _responseProvider = responseProvider;
            _responseNotifications = responseNotifications;
            _repositoryData = repositoryData;
            _watchQueue = watchQueue;
        }

        public void Run(IEnumerable<DictionaryConfig> configs)
        {
            _configurations = configs;
            _queueWatcher.OnError += _queueWatcher_OnError;
            _queueWatcher.OnSubscribe += QueueProvider_OnSubscribe;
            _queueWatcher.StartWatch(_watchQueue);
        }

        /// <summary>
        /// Обработка сообщений
        /// </summary>
        private async Task QueueProvider_OnSubscribe(IQueueMessage message)
        {
            Console.WriteLine($"Start processing message");
            Console.WriteLine(message.Body);
            var msgBody = JsonConvert.DeserializeObject<dynamic>(message.Body);
            var map = ((IDictionary<string, JToken>)msgBody)
                .ToDictionary(
                    x => x.Key,
                    x => string.IsNullOrWhiteSpace(x.Value.ToString()) ? null : x.Value.ToObject<object>());

            var config = _configurations.Single(x => x.ConfigName == map["ConfigName"].ToString());
            var queryResult = await ExecuteQueryAsync(map, config);

            if (queryResult.Any())
            {
                // prepare queryResult
                foreach (var item in queryResult)
                {
                    item.EntityType = config.EntityType;
                    if (!string.IsNullOrWhiteSpace(config.EntityKey))
                    {
                        item.EntityKey = config.EntityKey;
                    }
                }
                _responseNotifications.Send($"Send entity {config.EntityType} {queryResult.Count()}", $"{config.EntityType}");
                SendObjects(queryResult, config.EntityType);
            }

            Console.WriteLine("End processing message");
        }

        private void _queueWatcher_OnError(object sender, ExtThreadExceptionEventArgs e)
        {
            var msg = e.Message;

            if (msg.Errors == null)
            {
                msg.Errors = new List<string>();
            }

            msg.Errors.Add(e.Exception.GetBaseException().ToString());
            if (msg.Errors.Count < 3)
            {
                _queueProvider.PushMessage(msg);
            }
            else
            {
                _queueProviderErrors.PushMessage(msg);
            }
        }

        private async Task<IEnumerable<dynamic>> ExecuteQueryAsync(dynamic body, DictionaryConfig config)
        {
            _logger.Debug($"Start sql exec for entity type {config.EntityType}.");
            var objects = await _repositoryData.GetAllAsync(config.SqlString, body);
            _logger.Debug($"End sql exec for entity type {config.EntityType}.");

            return objects;
        }

        private void SendObjects(IEnumerable<dynamic> objects, string entityType)
        {
            if (objects.Count() > 1000)
            {
                Parallel.ForEach(objects, new ParallelOptions() { MaxDegreeOfParallelism = 50 }, item =>
                {
                    _responseProvider.Send(item, entityType);
                });
            }
            else
            {
                foreach (var item in objects)
                {
                    _responseProvider.Send(item, entityType);
                }
            }
        }
    }
}