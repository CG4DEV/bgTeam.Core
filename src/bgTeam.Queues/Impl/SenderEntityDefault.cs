namespace bgTeam.Queues.Impl
{
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class SenderEntityDefault : ISenderEntity
    {
        private bool _disposed = false;
        private IAppLogger _logger;
        private IQueueProvider _queueProvider;

        public SenderEntityDefault(
            IAppLogger logger,
            IQueueProvider queueProvider)
        {
            _logger = logger;
            _queueProvider = queueProvider;
        }

        ~SenderEntityDefault()
        {
            Dispose(false);
        }

        public void Send(IQueueMessage msg, params string[] queues)
        {
            SendQueue(msg, queues);
        }

        public void Send<T>(object entity, string entityType, params string[] queues)
            where T : IQueueMessage, new()
        {
            SendInternal<T>(entity, queues, entityType, null);
        }

        public void Send<T>(object entity, string entityType, int? delay, params string[] queues)
            where T : IQueueMessage, new()
        {
            SendInternal<T>(entity, queues, entityType, delay);
        }

        public void SendList<T>(IEnumerable<object> entities, string entityType, int? delay = null, params string[] queues)
            where T : IQueueMessage, new()
        {
            if (!entities.NullOrEmpty())
            {
                foreach (var entity in entities)
                {
                    SendInternal<T>(entity, queues, entityType, delay);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);

            // подавляем финализацию
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && _queueProvider != null)
                {
                    // Освобождаем управляемые ресурсы
                    _queueProvider.Dispose();
                }

                // освобождаем неуправляемые объекты
                _queueProvider = null;
                _logger = null;
                _disposed = true;
            }
        }

        private void SendInternal<T>(object entity, string[] queues, string entityType, int? delay)
            where T : IQueueMessage, new()
        {
            var str = MessageProviderDefault.ObjectToStr(entity);

            var mess = new T
            {
                Uid = Guid.NewGuid(),
                Body = str,
                Delay = delay,
            };

            SendQueue(mess, queues, entityType);
        }

        private void SendQueue(IQueueMessage mess, string[] queues, string entityType = "entity empty", int? delay = 1, int tryAttempt = 1)
        {
            try
            {
                _queueProvider.PushMessage(mess, queues);

                _logger.Info($"Success send entity - {entityType}");
            }
            catch (Exception exp)
            {
                if (tryAttempt < 5)
                {
                    _logger.Warning($"Failed send entity {mess.Body} after {tryAttempt} attempt. We will try again. Exception - {exp.Message}");

                    Thread.Sleep(2000);

                    // повторить попытку отправления
                    SendQueue(mess, queues, entityType, delay, ++tryAttempt);
                }
                else
                {
                    _logger.Error($"Failed send entity {mess.Body} after {tryAttempt} attempt. Lost entity. Exception - {exp.Message}");
                }
            }
        }
    }
}
