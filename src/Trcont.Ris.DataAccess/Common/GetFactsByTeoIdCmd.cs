namespace Trcont.Ris.DataAccess.Common
{
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Dto;

    public class GetFactsByTeoIdCmd : ICommand<GetFactsByTeoIdCmdContext, IEnumerable<FactByRouteDto>>
    {
        private readonly IRepository _repository;

        public GetFactsByTeoIdCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<FactByRouteDto> Execute(GetFactsByTeoIdCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<FactByRouteDto>> ExecuteAsync(GetFactsByTeoIdCmdContext context)
        {
            string sql = @"
                SELECT
                  f.OrderId,
                  f.ContNumber,
                  f.NaklNumber,
                  MAX(f.TransDate) AS TransDate,
                  MAX(f.ArrivalDate) AS ArrivalDate,
                  frm.IrsGuid AS FromPointGuid,
                  tp.IrsGuid AS ToPointGuid
                FROM OrdersFact f WITH(NOLOCK)
                  INNER JOIN vPoints frm WITH(NOLOCK) ON frm.Id = f.TransStationFromId
                  INNER JOIN vPoints tp WITH(NOLOCK) ON tp.Id = f.TransStationToId
                WHERE f.OrderId = @OrderId
                GROUP BY f.OrderId,
                  f.ContNumber,
                  f.NaklNumber,
                  frm.IrsGuid,
                  tp.IrsGuid";

            return await _repository.GetAllAsync<FactByRouteDto>(sql, context);
        }
    }
}
