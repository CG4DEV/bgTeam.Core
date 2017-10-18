namespace Trcont.Cud.DataAccess.Stations
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.CitTrans.Service;
    using Trcont.Cud.Domain.Entity.ExtInfo;

    public class GetContainerDislockCmd : ICommand<GetContainerDislockCmdContext, DislocationStation>
    {
        private readonly IAppLogger _logger;
        private readonly IMapperBase _mapper;
        private readonly ICitTransServiceClient _client;
        private readonly IRepository _repository;

        public GetContainerDislockCmd(IAppLogger logger, IMapperBase mapper, IRepository repository, ICitTransServiceClient client)
        {
            _logger = logger;
            _mapper = mapper;
            _client = client;
            _repository = repository;
        }

        public DislocationStation Execute(GetContainerDislockCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<DislocationStation> ExecuteAsync(GetContainerDislockCmdContext context)
        {
            var dislocations = await _client.GetOrderDislokAsync(context.OrderNumber, context.ContainerNumber);
            if (dislocations == null || !dislocations.Any())
            {
                return null;
            }

            var stationCode = dislocations.First().Station.StationCode;
            return await GetStationInfoAsync(stationCode.Substring(0, stationCode.Length - 1));
        }

        private async Task<DislocationStation> GetStationInfoAsync(string stationCode)
        {
            if (string.IsNullOrWhiteSpace(stationCode))
            {
                return null;
            }

            var sql = @"SELECT
                          r.ReferenceGUID,
                          r.ReferenceTitle AS 'StationTitle',
                          r.ReferenceAccount AS 'StationCode',
                          CASE
                            WHEN rs.PortGUID IS NULL THEN 0
                            ELSE 1
                          END AS 'IsPort' 
                        FROM Reference r
                          INNER JOIN RefStations rs ON r.ReferenceGUID = rs.ReferenceGUID
                        WHERE r.ReferenceAccount = @StationCode AND r.RefTypeId = 6";
            return await _repository.GetAsync<DislocationStation>(sql, new { StationCode = stationCode });
        }
    }
}
