namespace bgTeam.OrdersProducer.Jobs
{
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore.Common;
    using bgTeam.ProduceMessages;
    using bgTeam.Queues;
    using Quartz;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class OrdersJob : IJob
    {
        private static readonly object _lockObject = new object();

        public Task Execute(IJobExecutionContext context)
        {
            return ExecuteAsync(context);
        }

        protected void SendItem(IAppLogger logger, ISenderEntity sender, string entityType, string entityKey, dynamic item)
        {
            lock (_lockObject)
            {
                item.EntityType = entityType;
                if (!string.IsNullOrWhiteSpace(entityKey))
                {
                    item.EntityKey = entityKey;
                }
            }

            sender.Send(item, entityType);
        }

        protected static int? GetOffset(int? inputOffset)
        {
            int? offset = null;
            if (inputOffset.HasValue)
            {
                offset = inputOffset.Value < 0 ? inputOffset.Value : -inputOffset.Value;
            }

            return offset;
        }

        protected void BulkSend(IAppLogger logger, string entityType, string entityKey, IEnumerable<dynamic> objects, ISenderEntity sender)
        {
            if (objects.Count() > 1000)
            {
                Parallel.ForEach(objects, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, item =>
                {
                    SendItem(logger, sender, entityType, entityKey, item);
                });
            }
            else
            {
                foreach (var item in objects)
                {
                    SendItem(logger, sender, entityType, entityKey, item);
                }
            }
        }

        protected void SendNotification(ISenderEntity notificationSender, string body)
        {
            string timeMessage = $"{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")}: {body}";
            var msg = new QueueMessage(timeMessage);
            notificationSender.Send(msg, "Notification");
        }

        public async Task ExecuteAsync(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            var logger = (IAppLogger)dataMap["Logger"];
            var repository = (IRepositoryData)dataMap["Repository"];
            var sender = (ISenderEntity)dataMap["Sender"];
            var notificationSender = (ISenderEntity)dataMap["NotificationSender"];
            var groupConfig = (GroupConfig)dataMap["GroupConfig"];

            Log(logger, notificationSender, $"Start job for {groupConfig.GroupName}");
            foreach (var config in groupConfig.Configs)
            {
                try
                {
                    await ProcessAsync(config, logger, repository, sender, notificationSender);
                }
                catch (Exception ex)
                {
                    logger.Info($"Error in job for group {groupConfig.GroupName} and for entity type {config.EntityType}: {ex.ToString()}");
                }
            }

            Log(logger, notificationSender, $"End job for {groupConfig.GroupName}");
        }

        private async Task ProcessAsync(GroupElement config, IAppLogger logger, IRepositoryData repository, ISenderEntity sender, ISenderEntity notificationSender)
        {
            switch(config.EntityType)
            {
                case "Orders":
                    {
                        await ProcessOrdersAsync(config, logger, repository, sender, notificationSender);
                        break;
                    }
                case "OrdersFact":
                    {
                        await ProcessOrdersFactAsync(config, logger, repository, sender, notificationSender);
                        break;
                    }
                default:
                    {
                        logger.Error($"Unknown entity type {config.EntityType}");
                        throw new Exception($"Unknown entity type {config.EntityType}");
                    }
            }
        }

        private async Task ProcessOrdersAsync(GroupElement config, IAppLogger logger, IRepositoryData repository, ISenderEntity sender, ISenderEntity notificationSender)
        {
            int? dateOffset = GetOffset(config.DateChangeOffset);
            int i = 0;
            var param = new SqlParams
            {
                DateChange = dateOffset.HasValue ? DateTime.Now.AddDays(dateOffset.Value) : new DateTime(1900, 01, 01),
                PageSize = config.Pool,
                Skip = i * config.Pool
            };
            var preparedSql = GetPoolPagingSql(config.Sql, param);

            while (i == 0 || config.Pool > 0)
            {
                try
                {
                    logger.Debug($"Start sql exec for entity type {config.EntityType}. Step: {i}");

                    var objects = await repository.GetAllAsync(preparedSql, param);

                    if (!objects.Any())
                    {
                        SendNotification(notificationSender, $"Start job for entity type {config.EntityType}. No messages");
                        logger.Debug($"End job for entity type {config.EntityType}.");
                        return;
                    }

                    SendNotification(notificationSender, $"Start job for entity type {config.EntityType}. Messages count is {objects.Count()}");
                    param.Skip = ++i * config.Pool;
                }
                catch (Exception exp)
                {
                    logger.Error(exp);
                    throw;
                }
            }
        }

        private void Log(IAppLogger logger, ISenderEntity notificationSender, string msg)
        {
            logger.Info(msg);
            SendNotification(notificationSender, msg);
        }

        private async Task ProcessOrdersFactAsync(GroupElement config, IAppLogger logger, IRepositoryData repository, ISenderEntity sender, ISenderEntity notificationSender)
        {
            var objects = await GetFactInfoAsync(config, repository);

            if (!objects.Any())
            {
                SendNotification(notificationSender, $"Start job for entity type {config.EntityType}. No messages");
                return;
            }

            SendNotification(notificationSender, $"Start job for entity type {config.EntityType}. Messages count is {objects.Count()}");
            BulkSend(logger, config.EntityType, config.EntityKey, objects.ToArray(), sender);
            SendNotification(notificationSender, $"End job for entity type {config.EntityType}.");
        }

        private async Task<dynamic> GetFactInfoAsync(GroupElement config, IRepositoryData repository)
        {
            var sql = "EXEC dbo.Select_OrderFact_ForRis @DateChange";

            int? offset = GetOffset(config.DateChangeOffset);

            var param = new
            {
                DateChange = offset.HasValue ? DateTime.Now.AddDays(offset.Value) : new DateTime(1900, 01, 01)
            };

            return await repository.GetAllAsync(sql, param);
        }

        private string GetPoolPagingSql(string sql, SqlParams prms)
        {
            string offset = $"\r\nOFFSET @Skip ROWS FETCH FIRST @PageSize ROWS ONLY";
            string orderBy = "\r\nORDER BY 1";

            if (prms.PageSize > 0)
            {
                return Regex.IsMatch(sql, @"ORDER BY(\s|\w|\.)+$", RegexOptions.IgnoreCase) ? string.Concat(sql, offset) : string.Concat(sql, orderBy, offset);
            }
            else
            {
                return sql;
            }
        }

        private class SqlParams
        {
            public DateTime DateChange { get; set; }

            public int Skip { get; set; }

            public int PageSize { get; set; }
        }
    }
}
