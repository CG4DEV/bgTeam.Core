namespace bgTeam.DataAccess.Impl.Memory
{
    using System;

    public class CacheValue<T> : ICacheValue
    {
        public CacheValue()
        {
        }

        public CacheValue(T value, TimeSpan timeToExpired)
        {
            Value = value;
            ExpiredDateUtc = timeToExpired != TimeSpan.MaxValue ? DateTime.UtcNow.Add(timeToExpired) : DateTime.MaxValue;
        }

        public T Value { get; set; }

        public DateTime ExpiredDateUtc { get; set; }

        public bool IsExpire => ExpiredDateUtc <= DateTime.UtcNow;

        public void Refresh(TimeSpan newExpiredTime)
        {
            ExpiredDateUtc = newExpiredTime != TimeSpan.MaxValue ? DateTime.UtcNow.Add(newExpiredTime) : DateTime.MaxValue;
        }
    }
}
