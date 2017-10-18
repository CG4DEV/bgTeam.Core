using bgTeam;
using bgTeam.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trcont.Booking.DataAccess.Dictionaries;
using Trcont.Booking.EntityDataBaseRsi;
using Trcont.IRS.Common;
using Trcont.IRS.DataAccess.Dictionaries;
using Trcont.IRS.Domain.Dto;
using Trcont.IRS.Domain.Entity;

namespace Trcont.IRS.Story.Scripts
{
    public class SptListCNTStory : IStory<SptListCNTStoryContext, SptListCNTStoryDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IConnectionFactory _factory;
        private readonly IScriptSqlBuilder _scriptBuilder;

        public SptListCNTStory(
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

        public SptListCNTStoryDto Execute(SptListCNTStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<SptListCNTStoryDto> ExecuteAsync(SptListCNTStoryContext context)
        {
            var scriptId = 281072;//(int)ReferenceScripts.SptListCNT;

            var scripts = (await new GetScriptStepByReferenceIdCommand(_repository)
                .ExecuteAsync(new GetScriptStepByReferenceIdCommandContext { ReferenceId = scriptId }));

            var scriptParams = await _repositoryEn.GetAllAsync<ScriptParams>(x => x.ReferenceId == scriptId);

            var sqlList = _scriptBuilder.Build(scripts, scriptParams, context.ScriptParams);

            using (var connection = await _factory.CreateAsync())
            {
                var sql = sqlList.First();

                //return await repository.get(connection, sql);
                return null;
            }
        }
    }
}
