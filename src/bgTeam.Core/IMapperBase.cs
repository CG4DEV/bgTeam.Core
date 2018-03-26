namespace bgTeam
{
    using System;

    /// <summary>
    /// Интерфейс маппера
    /// </summary>
    public interface IMapperBase
    {
        /// <summary>
        /// Копирует значения свойств из объекта типа TSource в объект типа TDestination
        /// </summary>
        /// <typeparam name="TSource">Тип источника информации</typeparam>
        /// <typeparam name="TDestination">Тип приёмника информации</typeparam>
        /// <param name="source">Обект источника</param>
        /// <param name="destination">Объект приёмника</param>
        /// <returns>Объект приёмника</returns>
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

        TDestination Map<TDestination>(object source, TDestination destination, Type sourceType);
    }
}
