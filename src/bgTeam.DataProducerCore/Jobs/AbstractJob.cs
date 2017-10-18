namespace bgTeam.DataProducerCore.Jobs
{
    using bgTeam.DataAccess;
    using bgTeam.ProduceMessages;
    using bgTeam.Queues;
    using Quartz;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public abstract class AbstractJob : IJob
    {
        private static readonly object _lockObject = new object();

        public Task Execute(IJobExecutionContext context)
        {
            return ExecuteAsync(context);
        }

        public abstract Task ExecuteAsync(IJobExecutionContext context);

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

        protected static int? GetDateOffset(int? inputOffset)
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
                Parallel.ForEach(objects, new ParallelOptions() { MaxDegreeOfParallelism = 50 }, item =>
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

        protected virtual async Task ProcessAsync(IAppLogger logger, int? dateChangeOffset, string entityType, string entityKey, string sql, ISenderEntity sender, ISenderEntity notificationSender, IRepositoryData repository, int pool)
        {
            int? dateOffset = GetDateOffset(dateChangeOffset);
            int i = 0;
            var param = new SqlParams
            {
                DateChange = dateOffset.HasValue ? DateTime.Now.AddDays(dateOffset.Value) : new DateTime(1900, 01, 01),
                PageSize = pool,
                Skip = i * pool
            };
            var preparedSql = GetPoolPagingSql(sql, param);

            while (i == 0 || pool > 0)
            {
                try
                {
                    logger.Debug($"Start sql exec for entity type {entityType}. Step: {i}");

                    var objects = await repository.GetAllAsync(preparedSql, param);

                    if (!objects.Any())
                    {
                        SendNotification(notificationSender, logger, $"Start job for entity type {entityType}. No messages");
                        logger.Debug($"End job for entity type {entityType}.");
                        return;
                    }

                    SendNotification(notificationSender, logger, $"Start job for entity type {entityType}. Messages count is {objects.Count()}");
                    BulkSend(logger, entityType, entityKey, objects, sender);
                    param.Skip = ++i * pool;
                }
                catch (Exception exp)
                {
                    logger.Error(exp);
                    throw;
                }
            }
        }

        protected void SendNotification(ISenderEntity notificationSender, IAppLogger logger, string body)
        {
            logger.Info(body);
            string timeMessage = $"{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")}: {body}";
            var msg = new QueueMessage(timeMessage);
            notificationSender.Send(msg, "notification");
        }

        protected static string GetPoolPagingSql(string sql, SqlParams prms)
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

        public class SqlParams
        {
            public DateTime DateChange { get; set; }

            public int Skip { get; set; }

            public int PageSize { get; set; }
        }
    }
}