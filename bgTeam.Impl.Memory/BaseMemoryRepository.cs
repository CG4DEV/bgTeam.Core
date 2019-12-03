namespace bgTeam.DataAccess.Impl.Memory
{
    using System.Collections.Concurrent;
    using System.Linq;

    public class BaseMemoryRepository<TKey, TValue>
    {
        protected readonly ConcurrentDictionary<TKey, TValue> _container;

        public BaseMemoryRepository()
        {
            _container = new ConcurrentDictionary<TKey, TValue>();
        }

        protected virtual bool TryGetValue(TKey key, out TValue value)
        {
            var res = _container.TryGetValue(key, out value);
            return res;
        }

        protected virtual bool TrySetValue(TKey key, TValue value)
        {
            _container.AddOrUpdate(key, value, (k, v) => value);
            return true;
        }

        protected virtual bool Remove(TKey key)
        {
            return _container.TryRemove(key, out var _);
        }

        protected virtual IQueryable<TValue> GetAll()
        {
            return _container.Values.AsQueryable();
        }

        protected virtual int Count()
        {
            return _container.Count;
        }
    }
}
