namespace Trcont.IRS.Story.Scripts
{
    using bgTeam;
    using bgTeam.DataAccess;
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.Booking.DataAccess.Dictionaries;
    using Trcont.Booking.EntityDataBaseRsi;
    using Trcont.Domain;
    using Trcont.IRS.Common;
    using Trcont.IRS.DataAccess.Dictionaries;
    using Trcont.IRS.Domain.Dto;
    using Trcont.IRS.Domain.Entity;

    public class SptGetFactByTEOStory : IStory<SptGetFactByTEOStoryContext, IEnumerable<SptGetFactByTEODto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IConnectionFactory _factory;
        private readonly IScriptSqlBuilder _scriptBuilder;

        public SptGetFactByTEOStory(
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

        public IEnumerable<SptGetFactByTEODto> Execute(SptGetFactByTEOStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<SptGetFactByTEODto>> ExecuteAsync(SptGetFactByTEOStoryContext context)
        {
            var scriptId = (int)ReferenceScripts.SptGetFactByTEO;

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

        private static async Task<IEnumerable<SptGetFactByTEODto>> ExecuteScriptSql(System.Data.IDbConnection connection, string sql)
        {
            return await connection.QueryAsync<SptGetFactByTEODto>(sql);
        }
    }
}
