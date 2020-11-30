namespace bgTeam.StoryRunnerScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.DataAccess.Impl;
    using bgTeam.Impl.Rabbit;
    using bgTeam.Queues;
    using global::Quartz;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Main app job
    /// </summary>
    public class MainJob : IJob
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        /// <summary>
        /// Execute job
        /// </summary>
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            var logger = (ILogger<MainJob>)dataMap["Logger"];
            var repository = (IRepository)dataMap["Repository"];
            var sender = (ISenderEntity)dataMap["Sender"];
            var config = (JobTriggerInfo)dataMap["Config"];

            logger.LogInformation($"Start job for {config.ContextType}");

            try
            {
                IEnumerable<StoryRunnerMessageWork> sendItems = Enumerable.Empty<StoryRunnerMessageWork>();

                // Context value objects
                if (config.ContextValue != null)
                {
                    sendItems = await CreateSendObjects(config.ContextValue, config.ContextType);
                }

                // SQL
                if (!string.IsNullOrEmpty(config.SqlString))
                {
                    var sqlObject = new SqlObjectDefault(config.SqlString,
                    new
                    {
                        DateChangeFrom = config.DateChangeOffsetFrom.HasValue ? DateTime.Now.AddHours(config.DateChangeOffsetFrom.Value) : new DateTime(1900, 01, 01),
                        DateChangeTo = config.DateChangeOffsetTo.HasValue ? DateTime.Now.AddHours(config.DateChangeOffsetTo.Value) : new DateTime(1900, 01, 01),
                    });

                    sendItems = await CreateSendObjectsSQL(repository, sqlObject, config.ContextType);
                }

                logger.LogInformation($"Find send Items - {sendItems.Count()}");

                Parallel.ForEach(sendItems, new ParallelOptions() { MaxDegreeOfParallelism = 50 }, item =>
                {
                    sender.Send<QueueMessageRabbitMQ>(item, config.ContextType, config.NameQueue);
                });
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp, exp.Message);
            }

            logger.LogInformation($"End job for {config.ContextType}");
        }

        protected virtual async Task<IEnumerable<StoryRunnerMessageWork>> CreateSendObjects(
            dynamic[] contextValue,
            string contextType)
        {
            if (contextValue.Any())
            {
                return contextValue.Select(x => new StoryRunnerMessageWork
                {
                    Name = contextType,
                    Context = JsonConvert.SerializeObject(x, _settings),
                });
            }

            return Enumerable.Empty<StoryRunnerMessageWork>();
        }

        protected virtual async Task<IEnumerable<StoryRunnerMessageWork>> CreateSendObjectsSQL(
            IRepository repository,
            ISqlObject sqlObject,
            string contextType)
        {
            if (sqlObject != null && !string.IsNullOrEmpty(sqlObject.Sql))
            {
                var objects = await repository.GetAllAsync<dynamic>(sqlObject);

                if (objects.Any())
                {
                    return objects
                        .Select(x => new StoryRunnerMessageWork
                        {
                            Name = contextType,
                            Context = JsonConvert.SerializeObject(x, _settings),
                        });
                }

                return Enumerable.Empty<StoryRunnerMessageWork>();
            }
            else
            {
                return new[]
                {
                    new StoryRunnerMessageWork
                    {
                        Name = contextType,
                        Context = JsonConvert.SerializeObject(string.Empty),
                    },
                };
            }
        }
    }
}
