namespace bgTeam.Queues.Impl
{
    using System;

    public class SenderEntityDefault : ISenderEntity
    {
        private readonly IAppLogger _logger;
        private readonly IQueueProvider _queueProvider;

        public SenderEntityDefault(
            IAppLogger logger,
            IQueueProvider queueProvider)
        {
            _logger = logger;
            _queueProvider = queueProvider;
        }

        public void Send<T>(object entity, string entityKey)
            where T : IQueueMessage
        {
            throw new NotImplementedException();
        }

        public void Send<T>(object entity, string entityKey, int? delay)
            where T : IQueueMessage
        {
            throw new NotImplementedException();
        }

        public void Send<T>(object entity, string entityType, params string[] queues)
            where T : IQueueMessage
        {
            SendInternal<T>(entity, entityType, queues, null);
        }

        public void Send<T>(object entity, string entityType, int? delay, params string[] queues)
            where T : IQueueMessage
        {
            SendInternal<T>(entity, entityType, queues, delay);
        }

        private void SendInternal<T>(object entity, string entityType, string[] queues, int? delay, int tryAttempt = 1)
            where T : IQueueMessage
        {
            var str = MessageProviderDefault.ObjectToStr(entity);

            try
            {
                SendQueue<T>(str, entityType, queues, delay);
            }
            catch (Exception exp)
            {
                if (tryAttempt < 5)
                {
                    _logger.Warning($"Failed send entity {entity} after {tryAttempt} attempt. We will try again");

                    // повторить попытку отправления
                    SendInternal<T>(entity, entityType, queues, ++tryAttempt);
                }
                else
                {
                    _logger.Error($"Failed send entity {entity} after {tryAttempt} attempt. Lost entity");
                }
            }
        }

        private void SendQueue<T>(string str, string entityType, string[] queues, int? delay)
            where T : IQueueMessage
        {
            var mess = default(T);

            mess.Body = str;
            mess.Delay = delay;

            _queueProvider.PushMessage(mess, queues);

            _logger.Info($"Success send entity - {entityType}");
        }
    }
}
