namespace bgTeam.StoryRunner
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using bgTeam.Queues.Exceptions;
    using bgTeam.StoryRunner.Common;
    using Newtonsoft.Json;

    public class Runner
    {
        protected readonly IAppLogger _logger;
        protected readonly IQueueProvider _queueProvider;
        protected readonly IQueueWatcher<IQueueMessage> _queueWatcher;
        protected readonly IStoryProcessor _storyProcessor;

        protected readonly string _watchQueue;
        protected readonly string _errorQueue;

        public Runner(
            IAppLogger logger,
            IQueueProvider queueProvider,
            IQueueWatcher<IQueueMessage> queueWatcher,
            IStoryProcessor storyProcessor,
            IStoryRunnerSettings settings)
        {
            _logger = logger;
            _queueProvider = queueProvider;
            _queueWatcher = queueWatcher;
            _storyProcessor = storyProcessor;

            _watchQueue = settings.StoryQueue;
            _errorQueue = settings.ErrorQueue;
        }

        public void Run()
        {
            _queueWatcher.Error += _queueWatcher_OnError;
            _queueWatcher.Subscribe += QueueProvider_OnSubscribe;
            _queueWatcher.StartWatch(_watchQueue);
        }

        /// <summary>
        /// Обработка сообщений
        /// </summary>
        protected virtual async Task QueueProvider_OnSubscribe(IQueueMessage message)
        {
            _logger.Debug("StoryRunner - start read message");
            _logger.Debug($"StoryRunner - {message.Body}");

            var context = JsonConvert.DeserializeObject<StoryRunnerMessageWork>(message.Body);

            await _storyProcessor.ProcessAsync(context);

            _logger.Debug("StoryRunner - End read message");
        }

        protected virtual void _queueWatcher_OnError(object sender, ExtThreadExceptionEventArgs e)
        {
            var msg = e.Message;

            if (msg.Errors == null)
            {
                msg.Errors = new List<string>();
            }

            msg.Errors.Add(e.Exception.GetBaseException().ToString());

            if (msg.Errors.Count < 3)
            {
                _queueProvider.PushMessage(msg, _watchQueue);
            }
            else
            {
                _queueProvider.PushMessage(msg, _errorQueue);
            }
        }
    }
}
