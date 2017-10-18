namespace bgTeam.DataReceivingService.QueryProviders.Processor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam.Extensions;
    using bgTeam.ProcessMessages;
    using bgTeam.Queues;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class MessageProcessor : IMessageProcessor
    {
        private readonly IEnumerable<IQueryFactory> _factories;
        private readonly IQueryFactory _defaultFactory;

        public MessageProcessor(IQueryFactory defaultFactory, IEnumerable<IQueryFactory> factories)
        {
            _defaultFactory = defaultFactory.CheckNull(nameof(defaultFactory));
            _factories = factories;
        }

        public IQuery Process(IQueueMessage message)
        {
            return ProcessAsync(message).Result;
        }

        public async Task<IQuery> ProcessAsync(IQueueMessage message)
        {
            message.CheckNull(nameof(message));

            if (_factories != null && _factories.Any())
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(message.Body);
                var map = ((IDictionary<string, JToken>)obj)
                .ToDictionary(
                    x => x.Key,
                    x => string.IsNullOrWhiteSpace(x.Value.ToString()) ? null : x.Value.ToObject<object>()
                );

                string entityType = map["EntityType"].ToString();
                var factory = _factories.SingleOrDefault(x => x.ForMessageType == entityType);
                if (factory != null)
                {
                    return await factory.CreateQueryAsync(message);
                }
            }

            return await _defaultFactory.CreateQueryAsync(message);
        }
    }
}
