namespace bgTeam.DataProducerService.NCron.Jobs
{
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore.Jobs;
    using bgTeam.ProduceMessages;
    using Quartz;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommonJob : AbstractJob, IJob
    {
        public override async Task ExecuteAsync(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            var logger = (IAppLogger)dataMap["Logger"];
            var repository = (IRepositoryData)dataMap["Repository"];
            var sender = (ISenderEntity)dataMap["Sender"];
            var notificationSender = (ISenderEntity)dataMap["NotificationSender"];
            var sql = dataMap.GetString("Sql");
            string entityType = dataMap.GetString("EntityType");
            string entityKey = dataMap.GetString("EntityKey");
            int pool = dataMap.GetIntValue("Pool");

            logger.Info($"Start job for {entityType}");
            try
            {
                await ProcessAsync(logger, (int?)dataMap["DateChangeOffset"], entityType, entityKey, sql, sender, notificationSender, repository, pool);
            }
            catch (Exception ex)
            {
                logger.Info($"Error in job for {entityType}: {ex.ToString()}");
            }

            logger.Info($"End job for {entityType}");
        }
    }
}
