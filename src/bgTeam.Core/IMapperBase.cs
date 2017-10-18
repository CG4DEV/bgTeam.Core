namespace bgTeam
{
    public interface IMapperBase
    {
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
