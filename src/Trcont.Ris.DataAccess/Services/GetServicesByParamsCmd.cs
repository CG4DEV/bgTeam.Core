namespace Trcont.Ris.DataAccess.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public class GetServicesByParamsCmd : ICommand<GetServicesByParamsCmdContext, PriceServiceDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        private IEnumerable<Guid> _ignoreList;

        public GetServicesByParamsCmd(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;

            _ignoreList = new List<Guid>
            {
                new Guid("0000005b-0000-0000-0000-000000000000"), //11. Адрес склада
                new Guid("aa91460a-248c-4952-8106-8ac820724bd3"), //15. Масса брутто
                //new Guid("69cd13d5-4d8e-4fff-9ce7-984aa10b2463"), //18. Единица измерения
            };
        }

        public PriceServiceDto Execute(GetServicesByParamsCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<PriceServiceDto> ExecuteAsync(GetServicesByParamsCmdContext context)
        {
            if (context.Attributes == null || !context.Attributes.Any())
            {
                return null;
            }

            var i = 0;
            var select = new StringBuilder();
            var inner = new StringBuilder();
            var where = new StringBuilder();
            foreach (var item in context.Attributes.OrderByDescending(x => x.IsRequired))
            {
                if (_ignoreList.Contains(item.AttGuid))
                {
                    continue;
                }

                i++;
                select.Append($" t{i}.ValueGuid as value{i},");
                inner.AppendLine($"  INNER JOIN PriceServicePriceParam t{i} ON t{i}.PriceServiceId = ps.Id  AND t{i}.ServiceParamGuid = '{item.AttGuid}'");

                if (item.Value != null)
                {
                    if (item.IsRequired)
                    {
                        where.AppendLine($"  AND (t{i}.ValueGuid = '{item.Value}')");
                    }
                    else
                    {
                        where.AppendLine($"  AND (t{i}.ValueGuid = '{item.Value}' OR t{i}.ValueGuid IS NULL)");
                    }
                }
            }

            var sql =
$@"SELECT ps.*,{ select.ToString().TrimEnd(',') }        
FROM PriceService ps 
  INNER JOIN Price p ON ps.PriceId = p.Id 
{inner.ToString()}
WHERE ps.ServiceId = @ServiceId --AND p.PriceTypeId = 207754 -- прайс лист
{where.ToString()}";

            var srvMass = await _repository.GetAllAsync<dynamic>(sql, new { context.ServiceId });
            if (!srvMass.Any())
            {
                _logger.Debug($"For Service {context.ServiceId} not find ServicePrice");
                return null;
            }

            // анализируем услуги по заполнености
            var list = new List<PriceServiceDto>();
            foreach (var item in srvMass)
            {
                var itemDic = item as IDictionary<string, object>;

                var fulling = 0;
                for (int y = 1; y <= i; y++)
                {
                    if (itemDic[$"value{y}"] != null)
                    {
                        fulling++;
                    }
                }

                list.Add(new PriceServiceDto()
                {
                    Priority = fulling,

                    Id = Convert.ToInt32(itemDic["Id"]),
                    PriceId = Convert.ToInt32(itemDic["PriceId"]),
                    ServiceId = Convert.ToInt32(itemDic["ServiceId"]),
                    CurrencyId = Convert.ToInt32(itemDic["CurrencyId"]),
                    UnitId = Convert.ToInt32(itemDic["UnitId"]),
                    Rate = Convert.ToDecimal(itemDic["Rate"]),
                    RateVAT = Convert.ToDecimal(itemDic["RateVAT"]),
                    TarifType = Convert.ToInt32(itemDic["TarifType"]),
                    
                    //RateStepIncrement = (decimal?)itemDic["RateStepIncrement"],
                    //WeightRateBorder = (decimal?)itemDic["WeightRateBorder"],
                    //WeightStepIncrement = (decimal?)itemDic["WeightStepIncrement"],
                });
            }

            var prioritet = list.Max(x => x.Priority);

            return list
                .Where(x => x.Priority == prioritet)
                .OrderByDescending(x => x.Rate)
                .First();
        }
    }
}
