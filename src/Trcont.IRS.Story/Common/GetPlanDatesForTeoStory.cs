namespace Trcont.IRS.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System.Collections.Generic;
    using Trcont.IRS.Domain.Dto;
    using System.Threading.Tasks;
    using DapperExtensions.Mapper;

    public class GetPlanDatesForTeoStory : IStory<GetPlanDatesForTeoStoryContext, IEnumerable<PlanDatesForTeo>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetPlanDatesForTeoStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<PlanDatesForTeo> Execute(GetPlanDatesForTeoStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<PlanDatesForTeo>> ExecuteAsync(GetPlanDatesForTeoStoryContext context)
        {
            var sql = @"
                SELECT 
                  R.ReferenceGuid,
                  OG.PlanDate
                FROM Reference R
                  INNER JOIN vClientTransport CT ON CT.ReferenceId = R.ReferenceId
                  INNER JOIN vdetailtransport DT ON CT.ReferenceId = DT.ReferenceId AND DT.DetailType = 0
                  INNER JOIN vOrderGraph OG ON OG.DetailTransId_Plan = DT.DetailTransId
                  INNER JOIN vRefDocuments RD ON RD.ReferenceId = OG.WorkPlanId
                  INNER JOIN vWorkPlans WP ON WP.ReferenceId = RD.ReferenceId
                  INNER JOIN Reference R_st ON R_st.ReferenceGUID = WP.AgencyGUID AND 
                    CASE
                      WHEN OG.GraphType = 0 THEN DT.StationFromId
                      ELSE DT.StationToId
                    END = R_st.ReferenceId
                WHERE R.ReferenceGUID IN (SELECT Id FROM @ReferenceGUIDs) AND OG.WorkPlanId IS NOT NULL
                UNION ALL
                SELECT
                  R.ReferenceGuid,
                  OG.PlanDate
                FROM Reference R
                  INNER JOIN vClientTransport CT ON CT.ReferenceId = R.ReferenceId
                  INNER JOIN vdetailtransport DT ON CT.ReferenceId = DT.ReferenceId AND DT.DetailType = 0
                  INNER JOIN vOrderGraph OG ON OG.DetailTransId_Plan = DT.DetailTransId
                WHERE R.ReferenceGUID IN (SELECT Id FROM @ReferenceGUIDs) AND OG.WorkPlanId IS NULL
            ";

            return await _repository.GetAllAsync<PlanDatesForTeo>(sql, new
            {
                ReferenceGUIDs = new GuidDbType(context.ReferenceGuids)
            });
        }
    }
}
