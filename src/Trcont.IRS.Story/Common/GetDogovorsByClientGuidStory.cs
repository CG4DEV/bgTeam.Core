using bgTeam;
using bgTeam.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trcont.IRS.Domain.Dto;

namespace Trcont.IRS.Story.Common
{
    public class GetDogovorsByClientGuidStory : IStory<GetDogovorsByClientGuidStoryContext, IEnumerable<ClientDogovorDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetDogovorsByClientGuidStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<ClientDogovorDto> Execute(GetDogovorsByClientGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<ClientDogovorDto>> ExecuteAsync(GetDogovorsByClientGuidStoryContext context)
        {
            var sql =
                @"SELECT TOP 1000 
                      rd.ReferenceId AS Id,
                      rd.RegNumber,
                      r1.ReferenceTitle AS Name,
                      r1.ReferenceGUID AS ContractGuid,
                      rd.CreateDate AS ContractDate,
                      rd.BeginDate,
                      rd.EndDate,
                      r.ReferenceId AS ClientId,
                      r.ReferenceTitle AS ClientName,
                      (SELECT TOP 1 1 FROM vRefState rs WHERE rs.referenceid = rd.ReferenceId AND rs.StateId = 207374) AS IsValid
                    FROM vreference r
                    INNER JOIN vrefdogovors rd ON r.referenceid = rd.dogovorsideid
                    INNER JOIN vReference r1 ON rd.ReferenceId = r1.ReferenceId
                    INNER JOIN vRefTemplates rt ON rd.DocTypeId = rt.ReferenceId
                    WHERE r.referenceguid = @ClientGuid
                      AND rt.TemplateTypeId IN (9)
                    ORDER BY rd.EndDate";

            return await _repository.GetAllAsync<ClientDogovorDto>(sql, new { ClientGuid = context.ClientGuid });
        }
    }
}
