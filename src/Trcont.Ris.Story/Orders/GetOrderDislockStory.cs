namespace Trcont.Ris.Story.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.CitTrans.Service;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.DataAccess.Order;

    public class GetOrderDislockStory : IStory<GetOrderDislockStoryContext, IEnumerable<DislocationInfoDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IMapperBase _mapper;
        private readonly ICitTransServiceClient _client;
        private readonly IRepository _repository;

        public GetOrderDislockStory(
            IAppLogger logger,
            IMapperBase mapper,
            ICitTransServiceClient client,
            IRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _client = client;
            _repository = repository;
        }

        public IEnumerable<DislocationInfoDto> Execute(GetOrderDislockStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<DislocationInfoDto>> ExecuteAsync(GetOrderDislockStoryContext context)
        {
            /*var mock = new DislocationInfoDto()
            {
                ContNumber = string.IsNullOrWhiteSpace(context.ContainerNumber) ? "RZDU0133445" : context.ContainerNumber,
                Date = DateTime.Now,
                Latitude = 56.643500M,
                Longitude = 124.756200M,
                Operation = new ValueDto()
                {
                    Code = 0,
                    Name = "не определена"
                },
                Station = new StationDto()
                {
                    Country = new ValueDto()
                    {
                        Code = 643,
                        Name = "Россия"
                    },
                    StationCode = "911501",
                    StationName = "НЕРЮНГРИ-ПАССАЖИРСКАЯ"
                }
            };

            return new[] { mock };*/

            if (string.IsNullOrWhiteSpace(context.ContainerNumber) && string.IsNullOrWhiteSpace(context.OrderNumber))
            {
                string str = $"{nameof(context.ContainerNumber)} и {nameof(context.OrderNumber)}";
                throw new ArgumentNullException(str, $"{str} не могут быть одновременно пустыми.");
            }

            var dislocations = await _client.GetOrderDislokAsync(context.OrderNumber, context.ContainerNumber);
            if (dislocations == null || !dislocations.Any())
            {
                return Enumerable.Empty<DislocationInfoDto>();
            }

            var dislocDto = dislocations.Select(x => _mapper.Map(x, new DislocationInfoDto())).ToArray();
            var cmd = new GetRailStationByCodesCmd(_repository);
            var railStations = await cmd.ExecuteAsync(new GetRailStationByCodesCmdContext()
            {
                Codes = dislocDto.Select(x => x.Station.StationCode.Substring(0, 5)).ToArray()
            });

            return dislocDto.Select(x =>
            {
                var rs = railStations.FirstOrDefault(y => x.Station.StationCode.Substring(0, 5).Equals(y.Code.ToString()));
                x.Latitude = rs?.Latitude;
                x.Longitude = rs?.Longitude;
                return x;
            });
        }
    }
}
