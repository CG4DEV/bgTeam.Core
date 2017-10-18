namespace Trcont.App.Service
{
    using AutoMapper;
    using TKApplicationService;
    using Trcont.App.Service.Entity;
    using Trcont.Domain.Entity;

    public static class AutoMapperAppService
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<KpDocuments, KpDocumentObject>();

            cfg.CreateMap<OrderInfo, TeoDocumentObject>();

            //cfg.CreateMap<TeoServiceObject, KpServiceObject>()
            

            cfg.CreateMap<OrderInfo, KpDocumentObject>();

            cfg.CreateMap<OrderServiceInfo, TeoServiceObject>();
        }
    }
}
