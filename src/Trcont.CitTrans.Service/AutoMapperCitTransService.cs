namespace Trcont.CitTrans.Service
{
    using AutoMapper;
    using Trcont.CitTrans.Service.CitTrans;
    using Trcont.CitTrans.Service.Entity;

    public static class AutoMapperCitTransService
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Values, Value>();

            cfg.CreateMap<CitTrans.Station, Entity.Station>();

            cfg.CreateMap<ContainerDislocationContainetDislocationInfo, DislocationInfo>()
               .ForMember(x => x.ContNumber, opt => opt.MapFrom(x => x.KontNumber))
               .ForMember(x => x.Date, opt => opt.MapFrom(x => x.OperDate))
               .ForMember(x => x.Station, opt => opt.MapFrom(x => x.OperStation));
        }
    }
}
