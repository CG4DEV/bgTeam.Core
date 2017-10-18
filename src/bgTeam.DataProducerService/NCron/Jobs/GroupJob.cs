namespace bgTeam.DataProducerService.NCron.Jobs
{
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore.Common;
    using bgTeam.DataProducerCore.Jobs;
    using bgTeam.ProduceMessages;
    using Quartz;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class GroupJob : AbstractJob, IJob
    {
        public override async Task ExecuteAsync(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            var logger = (IAppLogger)dataMap["Logger"];
            var repository = (IRepositoryData)dataMap["Repository"];
            var sender = (ISenderEntity)dataMap["Sender"];
            var notificationSender = (ISenderEntity)dataMap["NotificationSender"];
            var groupConfig = (GroupConfig)dataMap["GroupConfig"];

            logger.Debug($"Start job for {groupConfig.GroupName}");
            var orders = groupConfig.Configs.Select(x => x.GroupOrderBy).Distinct().ToArray();
            foreach (var order in orders)
            {
                var configs = groupConfig.Configs.Where(x => x.GroupOrderBy == order).ToArray();
                if (configs.Length == 1)
                {
                    await ProcessOneAsync(logger, groupConfig.GroupName, configs[0], sender, notificationSender, repository);
                }
                else
                {
                    var tasks = configs.Select(x => ProcessOneAsync(logger, groupConfig.GroupName, x, sender, notificationSender, repository)).ToArray();
                    Task.WaitAll(tasks);
                }
            }

            logger.Debug($"End job for {groupConfig.GroupName}");
        }

        public async Task ProcessOneAsync(IAppLogger logger, string groupName, GroupElement config, ISenderEntity sender, ISenderEntity notificationSender, IRepositoryData repository)
        {
            try
            {
                await ProcessAsync(logger, config.DateChangeOffset, config.EntityType, config.EntityKey, config.Sql, sender, notificationSender, repository, config.Pool);
            }
            catch (Exception ex)
            {
                logger.Info($"Error in job for group {groupName} and for entity type {config.EntityType}: {ex.ToString()}");
            }
        }

        //private void Log(IAppLogger logger, ISenderEntity notificationSender, string msg)
        //{
        //    SendNotification(notificationSender, logger, msg);
        //}
    }
}
