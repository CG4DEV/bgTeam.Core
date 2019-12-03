namespace bgTeam.DataAccess.Impl.Memory
{
    using System.Linq;

    public class MemoryRepository<TKey, TValue> : BaseMemoryRepository<TKey, TValue>, IMemoryRepository<TKey, TValue>
    {
        public MemoryRepository()
            : base()
        {
        }

        public virtual new int Count()
        {
            return base.Count();
        }

        public virtual new IQueryable<TValue> GetAll()
        {
            return base.GetAll();
        }

        public virtual new bool Remove(TKey key)
        {
            return base.Remove(key);
        }

        public virtual new bool TryGetValue(TKey key, out TValue value)
        {
            return base.TryGetValue(key, out value);
        }

        public virtual new bool TrySetValue(TKey key, TValue value)
        {
            return base.TrySetValue(key, value);
        }
    }
}
