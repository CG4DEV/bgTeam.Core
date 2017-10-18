namespace bgTeam.DataReceivingService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bgTeam.Exceptions.Args;
    using bgTeam.ProcessMessages;
    using bgTeam.Queues;

    internal class Runner
    {
        private readonly IAppLogger _logger;
        private readonly IQueueWatcher<IQueueMessage> _queueWatcher;
        private readonly IMessageProcessor _processor;
        private readonly IQueueProvider _queueProvider;
        private readonly IQueueProvider _queueProviderErrors;
        private readonly string _watchQueue;

        public Runner(
            IAppLogger logger,
            IQueueWatcher<IQueueMessage> queueWatcher,
            IMessageProcessor processor,
            IQueueProvider queueProvider,
            IQueueProvider queueProviderErrors,
            string watchQueue)
        {
            _logger = logger;
            _queueWatcher = queueWatcher;
            _processor = processor;
            _queueProvider = queueProvider;
            _queueProviderErrors = queueProviderErrors;
            _watchQueue = watchQueue;
        }

        public async Task Run()
        {
            _queueWatcher.OnError += _queueWatcher_OnError;
            _queueWatcher.OnSubscribe += QueueProvider_OnSubscribe;
            _queueWatcher.StartWatch(_watchQueue);
        }

        /// <summary>
        /// Обработка сообщений
        /// </summary>
        private async Task QueueProvider_OnSubscribe(IQueueMessage message)
        {
            Console.WriteLine("Start read message");
            Console.WriteLine(message.Body);
            var query = await _processor.ProcessAsync(message);
            await query.ExecuteAsync();
            Console.WriteLine("End read message");
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
    }
}