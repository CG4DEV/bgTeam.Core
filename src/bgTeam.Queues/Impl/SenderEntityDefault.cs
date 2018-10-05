namespace bgTeam.Queues.Impl
{
    using System;
    using System.Threading;

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

        //public void Send<T>(object entity, string entityType)
        //    where T : IQueueMessage
        //{
        //    throw new NotImplementedException();
        //}

        //public void Send<T>(object entity, string entityType, int? delay)
        //    where T : IQueueMessage
        //{
        //    throw new NotImplementedException();
        //}

        public void Send<T>(IQueueMessage msg, params string[] queues)
        {
            SendQueue(msg, queues);
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
                var mess = default(T);

                mess.Body = str;
                mess.Delay = delay;

                SendQueue(mess, queues, entityType);
            }
            catch (Exception)
            {
                if (tryAttempt < 5)
                {
                    _logger.Warning($"Failed send entity {entity} after {tryAttempt} attempt. We will try again");

                    Thread.Sleep(1000);

                    // повторить попытку отправления
                    SendInternal<T>(entity, entityType, queues, ++tryAttempt);
                }
                else
                {
                    _logger.Error($"Failed send entity {entity} after {tryAttempt} attempt. Lost entity");
                }
            }
        }

        private void SendQueue(IQueueMessage mess, string[] queues, string entityType = "entity empty")
        {
            _queueProvider.PushMessage(mess, queues);

            _logger.Info($"Success send entity - {entityType}");
        }
    }
}
