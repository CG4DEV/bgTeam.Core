using bgTeam.DataAccess.Impl.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.Memory
{
    public class CacheValueTests
    {
        [Fact]
        public void ForMaxValueTimespanShouldBeSetMaxExpireDate()
        {
            var value = new CacheValue<string>("value", TimeSpan.MaxValue);
            Assert.Equal(DateTime.MaxValue, value.ExpiredDateUtc);
        }

        [Fact]
        public void ForTimespanShouldBeSetExpireDate()
        {
            var value = new CacheValue<string>("value", new TimeSpan(1, 0, 0));
            Assert.Equal(DateTime.UtcNow.AddHours(1), value.ExpiredDateUtc, new TimeSpan(0, 0, 1));
        }

        [Fact]
        public void ForMaxValueTimespanRefreshShouldSetMaxExpireDate()
        {
            var value = new CacheValue<string>("value", new TimeSpan(0, 0, 0));
            value.Refresh(TimeSpan.MaxValue);
            Assert.Equal(DateTime.MaxValue, value.ExpiredDateUtc);
        }

        [Fact]
        public void RefreshShouldSetExpireDate()
        {
            var value = new CacheValue<string>("value", new TimeSpan(1, 0, 0));
            value.Refresh(new TimeSpan(1, 0, 0));
            Assert.Equal(DateTime.UtcNow.AddHours(1), value.ExpiredDateUtc, new TimeSpan(0, 0, 1));
        }

        [Fact]
        public void DefaultConstructor()
        {
            Assert.NotNull(new CacheValue<string>());
        }

        [Fact]
        public void IsExpire()
        {
            var value = new CacheValue<string>("value", new TimeSpan(0, 0, 0));
            Assert.True(value.IsExpire);

            value = new CacheValue<string>("value", new TimeSpan(0, 1, 0));
            Assert.False(value.IsExpire);

        }
    }
}
