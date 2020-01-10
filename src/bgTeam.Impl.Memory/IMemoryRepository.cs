namespace bgTeam.DataAccess.Impl.Memory
{
    using System.Linq;

    public interface IMemoryRepository<TKey, TValue>
    {
        /// <summary>
        /// Попытка получения данных
        /// </summary>
        /// <typeparam name="TKey">Тип ключа</typeparam>
        /// <typeparam name="TValue">Тип значения</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Возвращаемое значение</param>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Попытка добавить данные
        /// </summary>
        /// <typeparam name="TKey">Тип ключа</typeparam>
        /// <typeparam name="TValue">Тип значения</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Добавляемое значение</param>
        bool TrySetValue(TKey key, TValue value);

        /// <summary>
        /// Безопасное удаление
        /// </summary>
        /// <typeparam name="TKey">Тип ключа</typeparam>
        /// <typeparam name="TValue">Тип значения</typeparam>
        bool Remove(TKey key);

        /// <summary>
        /// Получить коллекцию значений
        /// </summary>
        /// <typeparam name="TKey">Тип ключа</typeparam>
        /// <typeparam name="TValue">Тип значения</typeparam>
        IQueryable<TValue> GetAll();

        /// <summary>
        /// Получить количество элементов
        /// </summary>
        /// <typeparam name="TKey">Тип ключа</typeparam>
        /// <typeparam name="TValue">Тип значения</typeparam>
        int Count();
    }
}
