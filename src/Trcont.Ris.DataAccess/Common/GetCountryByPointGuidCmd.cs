namespace Trcont.Ris.DataAccess.Common
{
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public class GetCountryByPointGuidCmd : ICommand<GetCountryByPointGuidCmdContext, vPointsDto>
    {
        private readonly IRepository _repository;

        public GetCountryByPointGuidCmd(IRepository repository)
        {
            _repository = repository;
        }

        public vPointsDto Execute(GetCountryByPointGuidCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<vPointsDto> ExecuteAsync(GetCountryByPointGuidCmdContext context)
        {
            if (!context.PointGuid.HasValue)
            {
                throw new ArgumentNullException(nameof(context.PointGuid));
            }

            string sql = @"
                SELECT p.*, c.Title AS CountryName, c.Account CountryCode, c.IrsGuid  AS CountryGuid 
                  FROM vPoints p 
                  INNER JOIN Country c ON p.CountryId = c.Id
                  WHERE p.IrsGuid = @PointGuid";

            return await _repository.GetAsync<vPointsDto>(sql, context);
        }
    }
}
