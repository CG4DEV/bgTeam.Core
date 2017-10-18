namespace Trcont.Cud.DataAccess.Stations
{
    using bgTeam.DataAccess;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetExternalStationCodeCommand : ICommand<GetExternalStationCodeContext, string>
    {
        private readonly IRepository _repository;

        public GetExternalStationCodeCommand(IRepository repository)
        {
            _repository = repository;
        }

        public string Execute(GetExternalStationCodeContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<string> ExecuteAsync(GetExternalStationCodeContext context)
        {
             //var stationsList = await _repository.GetAllAsync<string>(@"
             //   SELECT p.ExternalCode
             //     FROM Reference r
             //     INNER JOIN Point p ON p.ReferenceCode like r.ReferenceAccount + '_'
             //   WHERE r.RefTypeId = 6 AND r.ReferenceGUID = @StationGUID
             //   AND (R.ActualBeginDate IS NULL OR R.ActualBeginDate < GETDATE())
             //   AND (R.ActualEndDate IS NULL OR R.ActualEndDate > GETDATE())", new { StationGUID = context.StationGUID });

            var stationsList = await _repository.GetAllAsync<string>(
                @"SELECT r.ExternalCode1 
                  FROM EPUPortal.dbo.Ref2ExternalSources r
                WHERE r.ExternalSource = 5 AND
                r.ReferenceGUID = @StationGUID", new { StationGUID = context.StationGUID });

            return stationsList.OrderBy(x => x.Length).FirstOrDefault();
        }
    }
}
