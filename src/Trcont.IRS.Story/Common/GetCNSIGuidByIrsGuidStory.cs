namespace Trcont.IRS.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.IRS.Domain.Dto;

    public class GetCNSIGuidByIrsGuidStory : IStory<GetCNSIGuidByIrsGuidStoryContext, IEnumerable<IrsGuidAndCNSIGuid>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetCNSIGuidByIrsGuidStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<IrsGuidAndCNSIGuid> Execute(GetCNSIGuidByIrsGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<IrsGuidAndCNSIGuid>> ExecuteAsync(GetCNSIGuidByIrsGuidStoryContext context)
        {
            var sql = @"
                SELECT
                  r.ReferenceGUID AS ReferenceGuid,
                  res.ExternalCodeGUID AS CNSIGuid,
                  r.ReferenceTitle AS Title,
                  r.ReferenceAccount AS Account
                FROM Reference r
                  LEFT JOIN Ref2ExternalSources res ON r.ReferenceId = res.ReferenceId AND res.ExternalSourceId IN (
                    44927018, --Код пункта ЦНСИ - Справочник кодов пунктов ЦНСИ ТК
                    45291654, --Единый код контрагента ЦНСИ - 7.2. Справочник «Контрагенты»
                    52504907, --Код ЦНСИ - Справочник кодов услуг 
                    52504928, --Код ЦНСИ - Справочник кодов стран
                    52504935, --Код ЦНСИ - Справочник кодов регионов
                    52504947, --Код ЦНСИ - Справочник кодов валют
                    52504967, --Код ЦНСИ - Справочник кодов типов контейнеров собственного парка
                    52504989, --Код ЦНСИ - Справочник кодов типов оборудования (контейнеры и вагоны)
                    52505005, --Код ЦНСИ - Справочник кодов типов вагонов собственного парка
                    52505034, --Код ЦНСИ - Спарвочник кодов типов вагонов парка ТК 
                    56920425 --Код ЦНСИ - Справочник кодов подразделений ТК
                    )
                WHERE r.ReferenceGUID IN (SELECT Id FROM @IrsGuids)
            ";

            return await _repository.GetAllAsync<IrsGuidAndCNSIGuid>(sql, new
            {
                IrsGuids = new GuidDbType(context.IrsGuids)
            });
        }
    }
}
