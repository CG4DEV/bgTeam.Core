namespace Trcont.IRS.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.IRS.Domain.Entity;

    public class GetExternalSourceStory : IStory<GetExternalSourceStoryContext, IEnumerable<ExternalSource>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetExternalSourceStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<ExternalSource> Execute(GetExternalSourceStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<ExternalSource>> ExecuteAsync(GetExternalSourceStoryContext context)
        {
            if (context.ExternalCodeGuids == null || !context.ExternalCodeGuids.Any())
            {
                return Enumerable.Empty<ExternalSource>();
            }

            var sql = @"SELECT * FROM Ref2ExternalSources
                        WHERE ExternalSourceId = @ExternalSourceId AND ReferenceId IN
                            (SELECT res.ReferenceId FROM Ref2ExternalSources res
                                INNER JOIN @ExternalCodeGuids ecs ON res.ExternalCodeGUID = ecs.Id
                            )";

            return await _repository.GetAllAsync<ExternalSource>(sql, 
                new { ExternalSourceId = context.ExternalSourceId, ExternalCodeGuids = new GuidDbType(context.ExternalCodeGuids) });
        }
    }
}
