namespace Trcont.IRS.Story
{
    using bgTeam;
    using bgTeam.DataAccess;
    using Dapper;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.Domain;
    using Trcont.IRS.Common;
    using Trcont.IRS.DataAccess.Dictionaries;
    using Trcont.IRS.Domain.Dto;
    using Trcont.IRS.Domain.Entity;

    public class SptGetCNTSummStory : IStory<SptGetCNTSummStoryContext, SptGetCNTSummDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IConnectionFactory _factory;
        private readonly IScriptSqlBuilder _scriptBuilder;

        public SptGetCNTSummStory(
            IAppLogger logger, 
            IRepository repository, 
            IRepositoryEntity repositoryEn, 
            IConnectionFactory factory, 
            IScriptSqlBuilder scriptBuilder)
        {
            _logger = logger;
            _repository = repository;
            _repositoryEn = repositoryEn;
            _factory = factory;
            _scriptBuilder = scriptBuilder;
        }

        public SptGetCNTSummDto Execute(SptGetCNTSummStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<SptGetCNTSummDto> ExecuteAsync(SptGetCNTSummStoryContext context)
        {
            var scriptId = (int)ReferenceScripts.SptGetCNTSumm;

            var scripts = (await new GetScriptStepByReferenceIdCommand(_repository)
                .ExecuteAsync(new GetScriptStepByReferenceIdCommandContext { ReferenceId = scriptId }));

            var scriptParams = await _repositoryEn.GetAllAsync<ScriptParams>(x => x.ReferenceId == scriptId);

            var sqlList = _scriptBuilder.Build(scripts, scriptParams, context.ScriptParams);

            using (var connection = await _factory.CreateAsync())
            {
                var sql = sqlList.First();

                return await ExecuteScriptSql(connection, sql);
            }
        }

        private static async Task<SptGetCNTSummDto> ExecuteScriptSql(System.Data.IDbConnection connection, string sql)
        {
            var result = new SptGetCNTSummDto();
            var multi = await connection.QueryMultipleAsync(sql);

            result.Services = multi.Read<SptGetCNTSummServicesInfo>();
            result.Orders = multi.Read<SptGetCNTSummOrdersInfo>();

            return result;
        }
    }
}
