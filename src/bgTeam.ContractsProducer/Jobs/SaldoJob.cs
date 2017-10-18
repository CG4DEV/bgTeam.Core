namespace bgTeam.ContractsProducer.Jobs
{
    using bgTeam.ContractsProducer.Dto;
    using bgTeam.ContractsProducer.ScriptBuilder;
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore.Jobs;
    using bgTeam.ProduceMessages;
    using Quartz;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using System.Data;

    public class SaldoJob : AbstractJob, IJob
    {
        private const int CONNECTION_TIMEOUT = 1200;

        public Task Execute(IJobExecutionContext context)
        {
            return ExecuteAsync(context);
        }

        public async override Task ExecuteAsync(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            var logger = (IAppLogger)dataMap["Logger"];
            //---------------------------------------------------------------
            var factory = (IConnectionFactory)dataMap["ConnectionFactory"];
            var scriptInfo = (ScriptTemplate)dataMap["ScriptInfo"];
            var scriptBuilder = (IScriptSqlBuilder)dataMap["ScriptBuilder"];
            var repository = (IRepository)dataMap["Repository"];
            //---------------------------------------------------------------
            var sender = (ISenderEntity)dataMap["Sender"];
            var notificationSender = (ISenderEntity)dataMap["NotificationSender"];
            var sql = dataMap.GetString("Sql");
            string entityType = dataMap.GetString("EntityType");
            string entityKey = dataMap.GetString("EntityKey");
            int pool = dataMap.GetIntValue("Pool");

            logger.Info($"Start job for {entityType}");
            try
            {
                await ProcessAsync(logger, (int?)dataMap["DateChangeOffset"], entityType, entityKey, sql, sender, 
                    notificationSender, pool, repository, scriptInfo, factory, scriptBuilder);
            }
            catch (Exception ex)
            {
                logger.Info($"Error in job for {entityType}: {ex.ToString()}");
            }

            logger.Info($"End job for {entityType}");
        }

        protected async Task ProcessAsync(IAppLogger logger, int? dateChangeOffset,
            string entityType, string entityKey, string sql, ISenderEntity sender, ISenderEntity notificationSender,
            int pool, IRepository repository, ScriptTemplate scriptInfo, IConnectionFactory factory, IScriptSqlBuilder scriptBuilder)
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

                    var isObjectsExists = await ProcessSaldoObjects(scriptInfo, repository, factory, scriptBuilder, preparedSql, param,
                        logger, entityType, entityKey, sender, notificationSender);

                    if (!isObjectsExists)
                    {
                        SendNotification(notificationSender, logger, $"Start job for entity type {entityType}. No messages");
                        logger.Debug($"End job for entity type {entityType}.");
                        return;
                    }

                    
                    param.Skip = ++i * pool;
                }
                catch (Exception exp)
                {
                    logger.Error(exp);
                    throw;
                }
            }
        }

        private async Task<bool> ProcessSaldoObjects(ScriptTemplate scriptInfo, IRepository repository, IConnectionFactory factory, IScriptSqlBuilder scriptBuilder, string preparedSql, SqlParams param,
            IAppLogger logger, string entityType, string entityKey, ISenderEntity sender, ISenderEntity notificationSender)
        {
            var guids = await repository.GetAllAsync<Guid>(preparedSql, param);
            if (!guids.Any())
            {
                return false;
            }

            SendNotification(notificationSender, logger, $"Start job for entity type {entityType}. Messages count is {guids.Count()}");

            Parallel.ForEach(guids, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, async contractGuid =>
            {
                try
                {
                    logger.Info($"Process Contract {contractGuid}");
                    var response = GetSaldoAsync(scriptInfo, contractGuid, factory, scriptBuilder).Result;
                    response.ContractGuid = contractGuid;

                    if (response.Orders != null && response.Orders.Any() ||
                        response.Services != null && response.Services.Any())
                    {
                        SendItem(logger, sender, entityType, entityKey, response);
                    }
                    else
                    {
                        logger.Info($"Contract {contractGuid}. Nothing to send");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error($"Process Contract {contractGuid}. Error: {ex.ToString()}");
                }
            });

            return true;
        }

        private async Task<SaldoInfoDto> GetSaldoAsync(ScriptTemplate scriptInfo, Guid contractGuid, IConnectionFactory factory, IScriptSqlBuilder scriptBuilder)
        {
            var myParams = new Dictionary<string, string>();

            myParams.Add("ContractGUID", $"'{contractGuid}'");
            myParams.Add("ReferenceGUID", "null");
            myParams.Add("LangGUID", "null");

            var sqlList = scriptBuilder.Build(scriptInfo.TemplateInfo, scriptInfo.ScriptParams, myParams);

            using (var connection = await factory.CreateAsync())
            {
                var sql = sqlList.First();
                return await ExecuteScriptSql(connection, sql);
            }
        }

        private async Task<SaldoInfoDto> ExecuteScriptSql(IDbConnection connection, string sql)
        {
            var result = new SaldoInfoDto();
            var multi = await connection.QueryMultipleAsync(sql, commandTimeout: CONNECTION_TIMEOUT);

            result.Services = multi.Read<SaldoSercviceInfoDto>();
            result.Orders = multi.Read<SaldoOrderInfoDto>();

            return result;
        }
    }
}
