namespace Trcont.IRS.Story
{
    using AutoMapper;
    using bgTeam;
    using Trcont.IRS.Domain.Dto;
    using Trcont.IRS.Domain.Entity;

    public class AutoMapperStory : IMapperBase
    {
        private readonly IMapper _mapper;

        public AutoMapperStory()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FactInfoDto, FactTransport>();
            });

            _mapper = config.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

    }
}
