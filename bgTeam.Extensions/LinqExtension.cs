namespace bgTeam.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Содержит расширения для работы с коллекциями
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        /// Возвращает уникальные элементы коллекции TSource, отобранные по ключевому полю TKey
        /// </summary>
        /// <typeparam name="TSource">тип объектов коллекции</typeparam>
        /// <typeparam name="TKey">Ключевое поле выбора</typeparam>
        /// <param name="source">Коллекция</param>
        /// <param name="keySelector">Функция селектор</param>
        /// <returns>Уникальные элементы, отобранные по ключевому полю</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Проверяет, что корллекция равна null или пуста
        /// </summary>
        /// <typeparam name="TSource">Тип коллекции</typeparam>
        /// <param name="source">Коллекция</param>
        /// <returns>True - коллекция пуста или равна null</returns>
        public static bool NullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }
    }
}
