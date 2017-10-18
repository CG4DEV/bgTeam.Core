namespace bgTeam.DataProducerCore
{
    using bgTeam.ProduceMessages;
    using bgTeam.Queues;
    using Newtonsoft.Json;
    using System;

    public class SenderEntity : ISenderEntity
    {
        private readonly IQueueProvider _queueProvider;
        private readonly IAppLogger _logger;

        public SenderEntity(IAppLogger logger, IQueueProvider queueProvider)
        {
            _queueProvider = queueProvider;
            _logger = logger;
        }

        public void Send(object entity, string entityKey)
        {
            var str = JsonConvert.SerializeObject(entity);
            // Отправка в очередь
            SendInternal(str, entityKey);
        }

        private void SendInternal(string entity, string entityKey, int tryAttempt = 1)
        {
            //_logger.Info($"Send entity {entity} attempt {tryAttempt}");
            try
            {
                SendQueue(entity, entityKey);
            }
            catch (Exception ex)
            {
                if (tryAttempt < 5)
                {
                    _logger.Error($"Failed send entity {entity} after {tryAttempt} attempt. We will try again");
                    SendInternal(entity, entityKey, ++tryAttempt);
                }
                else
                {
                    _logger.Error($"Failed send entity {entity} after {tryAttempt} attempt. Lost entity");
                }
            }

            //_logger.Info($"Success send entity {entity} after {tryAttempt} attempt");
        }

        private void SendQueue(string entity, string entityKey)
        {
            var mess = new QueueMessage(entity);

            _queueProvider.PushMessage(mess);

            _logger.Info($"Success send entity - {entityKey}");
        }
    }
}
