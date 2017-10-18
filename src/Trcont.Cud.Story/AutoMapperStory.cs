namespace Trcont.Cud.Story
{
    using App.Service;
    using AutoMapper;
    using bgTeam;
    using Trcont.App.Service.Entity;
    using Trcont.App.Service.TKApplicationService;
    using Trcont.CitTrans.Service;
    using Trcont.Cud.Domain.Common;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Domain.Entity.ExtInfo;
    using Trcont.Cud.Domain.Web.Dto;
    using Trcont.Cud.Story.Orders;
    using Trcont.Domain.Entity;
    //using Trcont.Fss.Service.Entity;

    public class AutoMapperStory : IMapperBase
    {
        private readonly IMapper _mapper;

        public AutoMapperStory()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Entity.Station, StationInfoDto>();

                cfg.CreateMap<SaveOrUpdateOrderStoryContext, KpDocuments>();

                cfg.CreateMap<SaveOrUpdateOrderStoryContext, OrderInfo>()
                   .ForMember(x => x.StationFromGuid, opt => opt.Ignore())
                   .ForMember(x => x.StationToGuid, opt => opt.Ignore())
                   .ForMember(x => x.ClientGuid, opt => opt.Ignore())
                   //.ForMember(x => x.EtsngGuid, opt => opt.Ignore())
                   //.ForMember(x => x.GngGuid, opt => opt.Ignore())
                   .ForMember(x => x.TrainTypeGuid, opt => opt.Ignore())
                   .ForMember(x => x.Comment, opt => opt.MapFrom(x => x.Comments));

                cfg.CreateMap<KpServices, KPServicesDto>();

                cfg.CreateMap<RouteDto, RouteTrain>()
                   .ForMember(x => x.FromPointGUID, opt => opt.MapFrom(x => x.From))
                   .ForMember(x => x.ToPointGUID, opt => opt.MapFrom(x => x.To));

                /*cfg.CreateMap<ServiceMandatory, RouteService>()
                   .ForMember(x => x.Code, opt => opt.MapFrom(x => x.USLCode))
                   .ForMember(x => x.CurrencyCode, opt => opt.MapFrom(x => x.Currency))
                   .ForMember(x => x.Summ, opt => opt.MapFrom(x => x.Cost))
                   .ForMember(x => x.TerritoryCode, opt => opt.MapFrom(x => x.Territory.ToString("000")));

                cfg.CreateMap<ServiceRecommended, RouteService>()
                   .ForMember(x => x.Code, opt => opt.MapFrom(x => x.USLCode))
                   .ForMember(x => x.CurrencyCode, opt => opt.MapFrom(x => x.Currency))
                   .ForMember(x => x.Summ, opt => opt.MapFrom(x => x.Cost))
                   .ForMember(x => x.TerritoryCode, opt => opt.MapFrom(x => x.Territory.ToString("000")));

                cfg.CreateMap<ExtraService, RouteService>()
                   .ForMember(x => x.Code, opt => opt.MapFrom(x => x.USLCode))
                   .ForMember(x => x.CurrencyCode, opt => opt.MapFrom(x => x.Currency))
                   .ForMember(x => x.Summ, opt => opt.MapFrom(x => x.Cost))
                   .ForMember(x => x.TerritoryCode, opt => opt.MapFrom(x => x.Territory.ToString("000")));*/

                //cfg.CreateMap<TeoServiceObjectExt, TeoServiceObject>();

                cfg.CreateMap<KpServices, RouteService>();

                cfg.CreateMap<CitTrans.Service.Entity.Value, Value>();

                cfg.CreateMap<CitTrans.Service.Entity.Station, Trcont.Cud.Domain.Entity.ExtInfo.Station>();

                cfg.CreateMap<CitTrans.Service.Entity.DislocationInfo, DislocationInfo>();

                cfg.CreateMap<OrderMinDto, TransPicDto>();

                cfg.CreateMap<FactInfo, BillInfo>();

                // инициализируем маппер для AppService
                AutoMapperAppService.Configure(cfg);

                // инициализируем маппер для CitTransService
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
