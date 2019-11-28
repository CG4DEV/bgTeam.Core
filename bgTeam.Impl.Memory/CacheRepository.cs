namespace bgTeam.DataAccess.Impl.Memory
{
    using System;
    using System.Linq;
    using System.Threading;

    public class CacheRepository<TKey, TValue> : InternalMemoryRepository<TKey, CacheValue<TValue>>, ICacheRepository<TKey, TValue>
    {
        protected const int _defaultCacheMinutes = 10;
        protected readonly bool _defaultIsTouched = false;
        protected readonly TimeSpan _expireTime;
        protected readonly Timer _timer;

        protected readonly IMemoryRepository<TKey, CacheValue<TValue>> _repository;

        public CacheRepository()
                    : this(new TimeSpan(0, _defaultCacheMinutes, 0), false)
        {
        }

        public CacheRepository(TimeSpan expireTime)
                    : this(expireTime, false)
        {
        }

        public CacheRepository(bool defaultIsTouched)
                    : this(new TimeSpan(0, _defaultCacheMinutes, 0), defaultIsTouched)
        {
        }

        public CacheRepository(TimeSpan expireTime, bool defaultIsTouched)
            : base()
        {
            _expireTime = expireTime;
            _repository = new MemoryRepository<TKey, CacheValue<TValue>>();
            _timer = new Timer(Clear, null, new TimeSpan(0, 0, 0), new TimeSpan(0, 2, 0));
        }

        public virtual bool TryGetValue(TKey key, out TValue value, bool? isTouched)
        {
            var result = base.TryGetValue(key, out CacheValue<TValue> cacheValue);
            if (!result || cacheValue.IsExpire)
            {
                value = default(TValue);
                return false;
            }

            isTouched = isTouched ?? _defaultIsTouched;
            if ((bool)isTouched)
            {
                cacheValue.Refresh(_expireTime);
            }

            value = cacheValue.Value;
            return result;
        }

        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            return this.TryGetValue(key, out value, null);
        }

        public virtual bool TrySetValue(TKey key, TValue value)
        {
            var newVal = new CacheValue<TValue>(value, _expireTime);
            return base.TrySetValue(key, newVal);
        }

        public virtual new int Count()
        {
            return base.Count();
        }

        public virtual new IQueryable<TValue> GetAll()
        {
            return base.GetAll().Select(x => x.Value);
        }

        public virtual new bool Remove(TKey key)
        {
            return base.Remove(key);
        }

        protected virtual void Clear(object state)
        {
            foreach (var item in _container)
            {
                if (item.Value.IsExpire)
                {
                    this.Remove(item.Key);
                }
            }
        }

        protected new bool TryGetValue(TKey key, out CacheValue<TValue> value)
        {
            return base.TryGetValue(key, out value);
        }

        protected new bool TrySetValue(TKey key, CacheValue<TValue> value)
        {
            return base.TrySetValue(key, value);
        }
    }
}
