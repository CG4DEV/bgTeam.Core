namespace Trcont.IRS.Story.Common
{
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.IRS.Domain.Dto;

    public class GetIRSGuidByNSIGuidStory : IStory<GetIRSGuidByNSIGuidStoryContext, GetIRSGuidByNSIGuidDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetIRSGuidByNSIGuidStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public GetIRSGuidByNSIGuidDto Execute(GetIRSGuidByNSIGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<GetIRSGuidByNSIGuidDto> ExecuteAsync(GetIRSGuidByNSIGuidStoryContext context)
        {
            var sql =
               @"SELECT
                  r.ReferenceGUID
                FROM Ref2ExternalSources r2e
                INNER JOIN Reference r ON r2e.ReferenceId = r.ReferenceId
                WHERE r2e.ExternalSourceId = 45291654 AND r2e.ExternalCodeGUID = @NSIGuid
            ";

            return await _repository.GetAsync<GetIRSGuidByNSIGuidDto>(sql, new { NSIGuid = context.NSIGuid });
        }
    }
}
