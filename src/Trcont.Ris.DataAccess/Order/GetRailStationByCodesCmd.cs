namespace Trcont.Ris.DataAccess.Order
{
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Entity;

    public class GetRailStationByCodesCmd : ICommand<GetRailStationByCodesCmdContext, IEnumerable<RailStation>>
    {
        private readonly IRepository _repository;

        public GetRailStationByCodesCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<RailStation> Execute(GetRailStationByCodesCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<RailStation>> ExecuteAsync(GetRailStationByCodesCmdContext context)
        {
            var prms = GetParams(context);
            return await _repository.GetAllAsync<RailStation>(GetSql(prms.Keys.ToArray()), prms);
        }

        private Dictionary<string, object> GetParams(GetRailStationByCodesCmdContext context)
        {
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < context.Codes.Length; i++)
            {
                dict.Add($"@Code{i}", context.Codes[i]);
            }

            return dict;
        }

        private string GetSql(params string[] sqlParams)
        {
            return $@"
                SELECT * FROM RailStation rs 
                WHERE rs.Code IN ({string.Join(",", sqlParams)})";
        }
    }
}
