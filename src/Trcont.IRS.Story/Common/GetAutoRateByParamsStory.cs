namespace Trcont.IRS.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Domain.Entity;
    using Trcont.IRS.Domain.Entity;

    public class GetAutoRateByParamsStory : IStory<GetAutoRateByParamsStoryContext, IEnumerable<AutoRates>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetAutoRateByParamsStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<AutoRates> Execute(GetAutoRateByParamsStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<AutoRates>> ExecuteAsync(GetAutoRateByParamsStoryContext context)
        {
            if (!context.ZoneGuid.HasValue)
            {
                throw new ArgumentNullException(nameof(context.ZoneGuid));
            }

            if (!context.PointGuid.HasValue)
            {
                throw new ArgumentNullException(nameof(context.PointGuid));
            }

            if (!context.ContainerTypeGuid.HasValue)
            {
                throw new ArgumentNullException(nameof(context.ContainerTypeGuid));
            }

            var sql =
                @"DECLARE @ServicesIds IntegerIdList

                Insert Into @ServicesIds
                SELECT ps.PriceServiceId FROM RefPriceDocuments pd
                  INNER JOIN RefPriceServices ps ON pd.ReferenceId = ps.ReferenceId 
                  --INNER JOIN Reference r ON r.ReferenceId = ps.WorkTypeId
                WHERE pd.PeriodBeginDate < GETDATE()
                  AND pd.PeriodEndDate > GETDATE()

                SELECT r.ReferenceId, r.ReferenceTitle, af.* FROM Traffic2.dbo.AutoFromRates af
                  INNER JOIN Reference r ON r.ReferenceGUID = af.ContainerTypeGUID
                  WHERE PointGUID = @PointGUID AND 
                    ZoneGUID = @ZoneGUID AND 
                    af.ContainerTypeGUID = @ContainerTypeGUID AND
                    af.PriceServiceId IN (SELECT * from @ServicesIds)
                  --ORDER BY af.ContainerTypeGUID -- af.PriceServiceId
                UNION
                SELECT r.ReferenceId, r.ReferenceTitle, at.* FROM Traffic2.dbo.AutoToRates at
                  INNER JOIN Reference r ON r.ReferenceGUID = at.ContainerTypeGUID
                  WHERE PointGUID = @PointGUID AND 
                    ZoneGUID = @ZoneGUID AND 
                    at.ContainerTypeGUID = @ContainerTypeGUID AND
                    at.PriceServiceId IN (SELECT * from @ServicesIds)
                  --ORDER BY at.ContainerTypeGUID --at.PriceServiceId";

            return await _repository.GetAllAsync<AutoRates>(sql, 
                new
                {
                    ZoneGUID = context.ZoneGuid,
                    PointGUID = context.PointGuid,
                    ContainerTypeGuid = context.ContainerTypeGuid
                });
        }
    }
}
