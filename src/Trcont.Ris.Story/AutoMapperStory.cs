namespace Trcont.Ris.Story
{
    using AutoMapper;
    using bgTeam;
    using Trcont.App.Service;
    using Trcont.App.Service.Entity;
    using Trcont.App.Service.TKApplicationService;
    using Trcont.CitTrans.Service;
    using Trcont.CitTrans.Service.CitTrans;
    using Trcont.CitTrans.Service.Entity;
    using Trcont.Domain;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Story.Orders;

    public class AutoMapperStory : IMapperBase
    {
        private readonly IMapper _mapper;

        public AutoMapperStory()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaveOrUpdateOrderStoryContext, OrderInfo>()
                   .ForMember(x => x.Comment, opt => opt.MapFrom(x => x.Comments));

                cfg.CreateMap<OrderServiceAttribute, LtrAttributeObject>()
                   .ForMember(x => x.PId, opt => opt.MapFrom(x => x.AttId))
                   .ForMember(x => x.ParamArrtibGUID, opt => opt.MapFrom(x => x.AttGuid))
                   .ForMember(x => x.ParamType, opt => opt.MapFrom(x => x.AttType))
                   .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Value))
                   .ForMember(x => x.PlaceRenderGuid, opt => opt.MapFrom(x => x.PlaceRender));

                cfg.CreateMap<RouteDto, RouteTrain>()
                   .ForMember(x => x.FromPointGUID, opt => opt.MapFrom(x => x.FromPointGUID))
                   .ForMember(x => x.ToPointGUID, opt => opt.MapFrom(x => x.ToPointGUID))
                   .ForMember(x => x.ArmIndex, opt => opt.MapFrom(x => x.ArmIndex));

                cfg.CreateMap<OrderServicesDto, RouteService>()
                    .ForMember(x => x.ServiceId, opt => opt.MapFrom(x => x.ServiceId))
                    .ForMember(x => x.RefServiceTypeGuid, opt => opt.MapFrom(y => y.ServiceTypeGuid));

                cfg.CreateMap<OrderMinDto, TransPicDto>();

                cfg.CreateMap<OrderServicesDto, RouteTrain>()
                    .ForMember(x => x.FromPointCode, opt => opt.MapFrom(x => x.FromPointCnsiCode))
                    .ForMember(x => x.ToPointCode, opt => opt.MapFrom(x => x.ToPointCnsiCode));

                cfg.CreateMap<Reference, Documents>()
                    .ForMember(x => x.Id, opt => opt.MapFrom(x => x.ReferenceId))
                    .ForMember(x => x.IrsGuid, opt => opt.MapFrom(x => x.ReferenceGUID))
                    .ForMember(x => x.DocumentsTypeId, opt => opt.MapFrom(x => (int)x.RefGroupId))
                    .ForMember(x => x.Code, opt => opt.MapFrom(x => x.ReferenceAccount))
                    .ForMember(x => x.Name, opt => opt.MapFrom(x => x.ReferenceTitle))
                    .ForMember(x => x.DocumentDate, opt => opt.MapFrom(x => x.CreateDate));

                cfg.CreateMap<OrderServiceInfoExt, KpServiceObject>();

                cfg.CreateMap<OrderInfoByGuid, OrderIds>();

                cfg.CreateMap<Value, ValueDto>();

                cfg.CreateMap<Trcont.CitTrans.Service.Entity.Station, StationDto>()
                    .ForMember(x => x.Country, opt => opt.MapFrom(x => x.Country));

                cfg.CreateMap<DislocationInfo, DislocationInfoDto>();

                cfg.CreateMap<OrderInfoByGuid, Order>();

                cfg.CreateMap<OrderInfo, Order>();

                cfg.CreateMap<DocumentDto, FileResponseDto>();

                // инициализируем маппер для Service
                AutoMapperAppService.Configure(cfg);
                AutoMapperCitTransService.Configure(cfg);
            });

            _mapper = config.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
