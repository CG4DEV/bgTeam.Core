namespace Trcont.Cud.Story.Orders
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.CitTrans.Service;
    using Trcont.Cud.Common;
    using Trcont.Cud.DataAccess.Dictionaries;
    using Trcont.Cud.DataAccess.Stations;
    using Trcont.Cud.Domain.Common;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Domain.Entity.ExtInfo;
    using Trcont.Cud.Domain.Enum;
    using Trcont.Domain.Entity;

    public class GetOrderTransPicStory : IStory<GetOrderTransPicStoryContext, TransPicDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IMapperBase _mapper;
        private readonly ITransPicService _transPicService;
        //private readonly ICitTransServiceClient _client;

        public GetOrderTransPicStory(
            IAppLogger logger,
            IRepository repository,
            IMapperBase mapper,
            ITransPicService transPicService/*,
            ICitTransServiceClient client*/
            )
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _transPicService = transPicService;
            //_client = client;
        }

        public TransPicDto Execute(GetOrderTransPicStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<TransPicDto> ExecuteAsync(GetOrderTransPicStoryContext context)
        {
            OrderMinDto order;
            IEnumerable<KpServices> services;

            if (!context.IsTeo)
            {
                order = await LoadKPAsync(context.OrderGuid);
                services = await new GetKPServicesByDocumentGuidCmd(_repository).ExecuteAsync(new GetKPServicesByDocumentGuidCmdContext(context.OrderGuid));
            }
            else
            {
                order = await LoadTEOAsync(context.OrderGuid);
                services = await new GetTEOServicesByDocumentGuidCmd(_repository).ExecuteAsync(new GetTEOServicesByDocumentGuidCmdContext(context.OrderGuid));
            }

            if (order == null)
            {
                return null;
            }

            var pointTypes = await GetPointTypesAsync(new Guid[] { order.PlaceFromGuid, order.PlaceToGuid });

            order.PlaceFromPointType = SetPointTypeField(order.PlaceFromGuid, pointTypes);
            order.PlaceToPointType = SetPointTypeField(order.PlaceToGuid, pointTypes);

            var routes = services.Select(x => new RouteDto(x.ParamPlaceFromGuid, x.ParamPlaceToGuid)).Distinct().ToArray();
            var collection = new RouteCollection();
            foreach (var route in routes)
            {
                var newRoute = _mapper.Map(route, new RouteTrain());

                newRoute.Services = services.Where(x => x.ParamPlaceFromGuid == route.From && x.ParamPlaceToGuid == route.To)
                    .Select(x => _mapper.Map(x, new RouteService()))
                    .ToArray();

                collection.Add(newRoute);
            }

            DislocationStation dislocation = null;

            //if (context.IsTeo)
            //{
            //    var cmd = new GetContainerDislockCmd(_logger, _mapper, _repository, _client);
            //    dislocation = await cmd.ExecuteAsync(new GetContainerDislockCmdContext { OrderNumber = order.Number });
            //}

            order.Routes = collection;
            order.TransPic = _transPicService.GetTransPicString(order, dislocation);

            return _mapper.Map(order, new TransPicDto());
        }

        private async Task<OrderMinDto> LoadKPAsync(Guid orderGuid)
        {
            var sql = @"
                SELECT 
                     d.ReferenceGUID,
                     d.Number,                      -- Номер
                     d.TrainTypeGUID,               -- Тип контейнера
                     d.Status,                      -- Статус
                     d.PlaceFromGUID,               -- Пункт отправления
                     d.PlaceToGUID,                 -- Пункт назначения
                     0 as IsTeo
                FROM KPDocuments d
                WHERE d.ReferenceGUID = @OrderGuid";

            return await _repository.GetAsync<OrderMinDto>(sql, new { OrderGuid = orderGuid });
        }

        private async Task<OrderMinDto> LoadTEOAsync(Guid orderGuid)
        {
            var sql = @"
                SELECT 
                     t.ReferenceGUID,
                     t.Number,                      -- Номер
                     t.TrainTypeGUID,               -- Тип контейнера
                     t.Status + 100,                -- Статус
                     t.PlaceFromGUID,               -- Пункт отправления
                     t.PlaceToGUID,                 -- Пункт назначения
                     1 as IsTeo
              FROM TEODocuments t
                WHERE t.ReferenceGUID = @OrderGuid";

            return await _repository.GetAsync<OrderMinDto>(sql, new { OrderGuid = orderGuid });
        }

        private async Task<IEnumerable<PointTypeDto>> GetPointTypesAsync(IEnumerable<Guid> placeGuids)
        {
            var cmd = new GetPointTypesCmd(_repository);
            return await cmd.ExecuteAsync(new GetPointTypesCmdContext() { PlaceGuids = placeGuids });
        }

        private PointTypeEnum SetPointTypeField(Guid guid, IEnumerable<PointTypeDto> refen)
        {
            if (guid != Guid.Empty)
            {
                var value = refen.FirstOrDefault(x => x.PlaceGuid == guid);
                if (value != null)
                {
                    return value.PointType;
                }
            }

            return PointTypeEnum.PointPortType;
        }
    }
}
