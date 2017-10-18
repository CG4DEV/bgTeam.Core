namespace Trcont.Ris.DataAccess.Services
{
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Entity;

    public class GetParamsValueForServicesCmd : ICommand<GetParamsValueForServicesCmdContext, IEnumerable<PriceServiceParam>>
    {
        private readonly IRepository _repository;

        public GetParamsValueForServicesCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<PriceServiceParam> Execute(GetParamsValueForServicesCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<PriceServiceParam>> ExecuteAsync(GetParamsValueForServicesCmdContext context)
        {
            if (!context.ServiceId.HasValue)
            {
                throw new ArgumentNullException(nameof(context.ServiceId));
            }

            if (!context.PointIrsGuid.HasValue)
            {
                throw new ArgumentNullException(nameof(context.PointIrsGuid));
            }

            var sql = @"SELECT DISTINCT
                          t1.ServiceParamGuid
                         ,t1.ValueText
                         ,t1.ValueGuid
                         ,t1.ValueNum
                         ,t1.ValueDate
                        FROM PriceServiceServiceParam t1
                        WHERE t1.PriceServiceId IN
                            ( SELECT t2.PriceServiceId
                             FROM PriceServiceServiceParam t2
                             INNER JOIN PriceService ps ON ps.Id = t2.PriceServiceId
                             WHERE t2.ServiceParamGuid = '0000004c-0000-0000-0000-000000000000'
                               AND ValueGuid = @Point
                               AND ps.ServiceId = @ServiceId )
                        ORDER BY t1.ServiceParamGuid";

            return await _repository.GetAllAsync<PriceServiceParam>(sql,
                new { ServiceId = context.ServiceId, Point = context.PointIrsGuid });
        }
    }
}
