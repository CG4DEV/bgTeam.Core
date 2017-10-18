namespace Trcont.Cud.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using bgTeam.Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.Cud.Common;
    using Trcont.Cud.DataAccess.Dictionaries;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Domain.Entity;
    using Trcont.Cud.Domain.Enum;
    using Trcont.Cud.Domain.Web.Context;
    using Trcont.Cud.Domain.Web.Dto;
    using Trcont.Domain.Common;

    public class GetOrdersByContractGuidStory : IStory<GetOrdersByContractGuidStoryContext, PageDto<OrderDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IWebClient _webClient;

        private const string GET_REFERENCE_BYGUID = "api/Info/GetReferenceByIRSGuid";
        private const string FILE_METHOD = "api/Info/GetTeoDocumentContent?={0}";
        private const string GET_FACT_TRANSPORT_BYGUID = "api/Info/GetFactTransportInfoByTeoGuid";
        private const string GET_CNSI_GUID_BY_IRS_GUID = "api/Info/GetCNSIGuidByIrsGuid";

        private const string GET_PLAN_DATES_FOR_TEO = "api/Info/GetPlanDatesForTeo";

        public GetOrdersByContractGuidStory(
            IAppLogger logger,
            IRepository repository,
            IWebClient webClient)
        {
            _logger = logger;
            _repository = repository;
            _webClient = webClient;
        }

        public PageDto<OrderDto> Execute(GetOrdersByContractGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<PageDto<OrderDto>> ExecuteAsync(GetOrdersByContractGuidStoryContext context)
        {
            var orders = await GetOrdersAsync(context);
            var totalCount = await GetOrdersCountAsync(context);

            if (!orders.Any())
            {
                return new PageDto<OrderDto>()
                {
                    Page = Enumerable.Empty<OrderDto>(),
                    TotalCount = totalCount
                };
            }

            orders.Where(x => x.IsTeo).DoForEach(x => x.FileLink = context.ServerUrl + string.Format(FILE_METHOD, x.ReferenceGuid));

            // Получить CNSI Guid
            var guids = new List<Guid?>();
            orders.DoForEach(x =>
            {
                guids.Add(x.PlaceFromGuid);
                guids.Add(x.PlaceToGuid);
                guids.Add(x.ETSNGGuid);
                guids.Add(x.ContainerTypeGuid);
            });

            var refen = await GetCNSIInfoByGuidsAsync(guids);

            // Получить наименования справочных значений
            guids.Clear();
            orders.DoForEach(x =>
            {
                guids.Add(x.PlaceFromGuid);
                guids.Add(x.PlaceToGuid);
                guids.Add(x.ETSNGGuid);
                guids.Add(x.ContainerTypeGuid);
            });

            var guids2 = guids.Where(x => x.HasValue).Select(x => x.Value).Distinct().ToList();
            var titles = await _webClient
                .PostAsync<IEnumerable<ReferenceInfo>>(GET_REFERENCE_BYGUID, new { ReferenceGuids = guids2 }) ?? Enumerable.Empty<ReferenceInfo>();

            var refGuids = orders.Where(x => x.IsTeo).Select(x => x.ReferenceGuid).ToArray();
            var facts = await _webClient
                .PostAsync<IEnumerable<FactResponse>>(GET_FACT_TRANSPORT_BYGUID, new { ReferenceGuids = refGuids }) ?? Enumerable.Empty<FactResponse>();

            var plansDates = await GetPlanDatesForTeoContextAsync(refGuids);

            orders.DoForEach(x =>
            {
                x.StatusTitle = ((OrderStatusEnum)x.Status).GetDescription();

                x.PlaceFromCNSIGuid = SetFieldCNSIId(x.PlaceFromGuid, refen);
                x.PlaceToCNSIGuid = SetFieldCNSIId(x.PlaceToGuid, refen);
                x.EtsngCNSIGuid = SetFieldCNSIId(x.ETSNGGuid, refen);
                x.ContainerTypeCNSIGuid = SetFieldCNSIId(x.ContainerTypeGuid, refen);

                x.PlaceFromTitle = SetFieldTitle(x.PlaceFromGuid, titles);
                x.PlaceToTitle = SetFieldTitle(x.PlaceToGuid, titles);
                x.ETSNGTitle = SetFieldTitle(x.ETSNGGuid, titles);

                x.ContainerTypeTitle = SetFieldTitle(x.ContainerTypeGuid, titles);

                if (x.IsTeo)
                {
                    SetFactInfo(x, facts);
                    x.PlanDates = plansDates
                        .Where(y => y.ReferenceGuid == x.ReferenceGuid)
                        .Select(y => y.PlanDate)
                        .ToArray();
                }

                x.Summ = x.Summ.RoundUp(2);
            });

            var result = new PageDto<OrderDto>()
            {
                Page = orders,
                TotalCount = totalCount
            };

            return result;
        }

        private async Task<IEnumerable<PlanDatesForTeoDto>> GetPlanDatesForTeoContextAsync(Guid[] refGuids)
        {
            return await _webClient.PostAsync<IEnumerable<PlanDatesForTeoDto>>(GET_PLAN_DATES_FOR_TEO,
                new GetPlanDatesForTeoContext() { ReferenceGuids = refGuids })
                ?? Enumerable.Empty<PlanDatesForTeoDto>();
        }

        private async Task<IEnumerable<IrsGuidAndCNSIGuidDto>> GetCNSIInfoByGuidsAsync(List<Guid?> guids)
        {
            var cleanGuids = guids.Where(x => x.HasValue).Select(x => x.Value).Distinct().ToArray();
            return await _webClient.PostAsync<IEnumerable<IrsGuidAndCNSIGuidDto>>(GET_CNSI_GUID_BY_IRS_GUID,
                new GetCNSIGuidByIrsGuidContext() { IrsGuids = cleanGuids })
                ?? Enumerable.Empty<IrsGuidAndCNSIGuidDto>();
        }

        private void SetFactInfo(OrderDto order, IEnumerable<FactResponse> facts)
        {
            var fact = facts.FirstOrDefault(x => x.ReferenceGuid == order.ReferenceGuid);
            if (fact == null)
            {
                return;
            }

            var factInfo = fact.FactTransportCollection.FirstOrDefault(x => x.FactSourceId == (int)FactSourceEnum.Doc14);
            if (factInfo == null)
            {
                return;
            }

            order.ArrivalDate = factInfo.ComDate;
            order.SendDate = factInfo.OutDate;
        }

        private string SetFieldTitle(Guid? guid, IEnumerable<ReferenceInfo> titles)
        {
            if (guid.HasValue)
            {
                var value = titles.FirstOrDefault(x => x.ReferenceGuid == guid);
                if (value != null)
                {
                    return value.Value;
                }
            }

            return null;
        }

        private Guid? SetFieldCNSIId(Guid? guid, IEnumerable<IrsGuidAndCNSIGuidDto> refen)
        {
            if (guid.HasValue)
            {
                var value = refen.FirstOrDefault(x => x.ReferenceGuid == guid);
                if (value != null)
                {
                    return value.CNSIGuid;
                }
            }

            return null;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(GetOrdersByContractGuidStoryContext context)
        {
            var sql =
                @"SELECT 
                    d.ReferenceGUID,
                    d.RefDocumentGUID,
                    d.Number,
                    CASE
                        WHEN d.IsTeo = 1 THEN d.Status + 100
                        ELSE d.Status
                    END as 'Status',
                    d.PeriodBeginDate,
                    d.PeriodEndDate,
                    d.ClientGUID,
                    d.ClientName,
                    d.ContractNumber,
                    d.Summ,
                    d.PlaceFromGUID,
                    d.PlaceToGUID,
                    d.ETSNGGUID,
                    d.TrainTypeGUID as ContainerTypeGUID,
                    d.CreateDate,
                    d.LoadDate,
                    d.IsTeo
                FROM vDocuments d WHERE d.ContractGUID = @ContractGuid
                ORDER BY d.PeriodEndDate DESC
                OFFSET @First ROWS FETCH NEXT @PageSize ROWS ONLY
            ";

            return await _repository.GetAllAsync<OrderDto>(
                sql, new
                {
                    ContractGuid = context.ContractGuid,
                    First = PagingHelper.GetOffsetByPageNumber(context.PageNumber, context.PageSize),
                    PageSize = context.PageSize
                });
        }

        public async Task<int> GetOrdersCountAsync(GetOrdersByContractGuidStoryContext context)
        {
            var sql =
                @"SELECT 
                    COUNT(1)
                FROM vDocuments d WHERE d.ContractGUID = @ContractGuid
            ";

            return await _repository.GetAsync<int>(
                sql, new
                {
                    ContractGuid = context.ContractGuid
                });
        }

        class ReferenceInfo
        {
            public Guid ReferenceGuid { get; set; }

            public string Value { get; set; }
        }
    }
}
