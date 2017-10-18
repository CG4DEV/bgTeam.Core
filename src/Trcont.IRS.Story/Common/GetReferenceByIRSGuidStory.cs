namespace Trcont.IRS.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.Domain;
    using Trcont.IRS.Domain.Dto;

    public class GetReferenceByIRSGuidStory : IStory<GetReferenceByIRSGuidListStoryContext, IEnumerable<Reference>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetReferenceByIRSGuidStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<Reference> Execute(GetReferenceByIRSGuidListStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<Reference>> ExecuteAsync(GetReferenceByIRSGuidListStoryContext context)
        {
            if (context.ReferenceGuids == null || !context.ReferenceGuids.Any())
            {
                return Enumerable.Empty<Reference>();
            }

            var sql = "SELECT * FROM [dbo].[vReference] WHERE ReferenceGUID IN (SELECT Id FROM @ReferenceGuids)";
            return await _repository.GetAllAsync<Reference>(sql, new { ReferenceGuids = new GuidDbType(context.ReferenceGuids) });
        }
    }
}
