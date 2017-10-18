namespace bgTeam.ContractsProducer
{
    using bgTeam.ContractsProducer.Command;
    using bgTeam.ContractsProducer.Dto;
    using bgTeam.ContractsProducer.Entity;
    using bgTeam.ContractsProducer.Jobs;
    using bgTeam.ContractsProducer.ScriptBuilder;
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore.SchedulersFactory;
    using bgTeam.ProduceMessages;
    using Quartz;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SaldoSchedulersFactory : AbstractSchedulersFactory, IPluginSchedulersFactory
    {
        private readonly IAppLogger _logger;
        private readonly ISenderEntity _sender;
        private readonly ISenderEntity _notificationSender;

        private readonly IRepository _repository;
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IConnectionFactory _factory;

        private const int SCRIPT_ID = 347237;

        public SaldoSchedulersFactory(
            IAppLogger logger,
            ISenderEntity sender,
            ISenderEntity notificationSender,
            ISchedulerFactory schedulerFactory,
            IRepositoryEntity repositoryEn,
            IRepository repository,
            IConnectionFactory factory
            )
            : base(schedulerFactory)
        {
            _logger = logger;
            _repository = repository;
            _sender = sender;
            _notificationSender = notificationSender;
            _repositoryEn = repositoryEn;
            _factory = factory;
        }

        public string EntityType => "SaldoContracts";

        public string GroupName => null;

        public void Create(IDictionaryConfig config)
        {
            var name = $"{config.EntityType}{Guid.NewGuid()}";
            var scriptInfo = LoadScriptInfoAsync().Result;
            IJobDetail jobDetail = CreateJob<SaldoJob>(name, CreateCommonMap(config, scriptInfo));
            ITrigger trigger = CreateTrigger(config.EntityType, config.DateFormatStart, jobDetail);
            CreateScheduler(jobDetail, trigger);
        }

        public void CreateGroup(IEnumerable<IDictionaryConfig> configs)
        {
            throw new NotSupportedException();
        }

        private IDictionary<string, object> CreateCommonMap(IDictionaryConfig config, ScriptTemplate scriptTemplate)
        {
            return new Dictionary<string, object>()
            {
                { "EntityType", config.EntityType },
                { "EntityKey", config.EntityKey },
                { "DateChangeOffset", config.DateChangeOffset },
                { "Sql", config.SqlString },
                { "Logger", _logger },
                //-----------------------------------------------
                { "ScriptInfo", scriptTemplate },
                { "ConnectionFactory", _factory },
                { "ScriptBuilder", new ScriptSqlBuilder() },
                { "Repository", _repository },
                //-----------------------------------------------
                { "Sender", _sender },
                { "NotificationSender", _notificationSender },
                { "Pool", config.Pool }
            };
        }

        private async Task<ScriptTemplate> LoadScriptInfoAsync()
        {
            var scripts = (await new GetScriptStepByReferenceIdCommand(_repository)
                .ExecuteAsync(new GetScriptStepByReferenceIdCommandContext { ReferenceId = SCRIPT_ID }));

            var scriptParams = await _repositoryEn.GetAllAsync<ScriptParams>(x => x.ReferenceId == SCRIPT_ID);
            return new ScriptTemplate()
            {
                TemplateInfo = scripts,
                ScriptParams = scriptParams
            };
        }
    }
}
