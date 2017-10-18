namespace bgTeam.DataProducerService.NCron.Jobs
{
    using bgTeam.DataAccess;
    using bgTeam.DataProducerService.Infrastructure;
    using bgTeam.ProduceMessages;
    using Quartz;
    using System.Threading;
    using System.Threading.Tasks;

    public class DummyJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            var logger = (IAppLogger)dataMap["Logger"];
            var repository = (IRepositoryData)dataMap["Repository"];
            var sender = (ISenderEntity)dataMap["Sender"];
            var sql = dataMap.GetString("Sql");
            string entityType = dataMap.GetString("EntityType");
            string entityKey = dataMap.GetString("EntityKey");

            logger.Info($"Start dummy job for {entityType}");

            logger.Info($"logger is not null? - { logger != null }");
            logger.Info($"repository is not null? - { repository != null }");
            logger.Info($"sender is not null? - { sender != null }");
            logger.Info($"sql is not null? - { !string.IsNullOrWhiteSpace(sql) }");
            logger.Info($"entityType is not null? - { !string.IsNullOrWhiteSpace(entityType) }");
            logger.Info($"entityKey is not null? - { !string.IsNullOrWhiteSpace(entityKey) }");

            //Thread.Sleep(10000);
            await Task.Delay(10000);

            logger.Info($"End dummy job for {entityType}");
        }
    }
}
