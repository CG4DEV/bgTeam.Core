using bgTeam.DataAccess.Impl.Memory;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.Memory
{
    public class CacheRepositoryTests
    {
        [Fact]
        public async Task ValueShouldBeNotReturnedIfTimeIsExpired()
        {
            var repository = new CacheRepository<string, string>(new System.TimeSpan(0, 0, 0, 0, 20), true);
            Assert.True(repository.TrySetValue("key1", "value1"));

            Assert.True(repository.TryGetValue("key1", out string value));
            Assert.Equal("value1", value);

            await Task.Delay(50);
            Assert.False(repository.TryGetValue("key1", out _));
        }

        [Fact]
        public void Count()
        {
            var repository = new CacheRepository<string, string>();
            repository.TrySetValue("key1", "value1");
            repository.TrySetValue("key2", "value2");
            Assert.Equal(2, repository.Count());
        }

        [Fact]
        public void GetAll()
        {
            var repository = new CacheRepository<string, string>();
            repository.TrySetValue("key1", "value1");
            repository.TrySetValue("key2", "value2");
            var all = repository.GetAll();
            Assert.Equal(2, all.Count());
        }

        [Fact]
        public void Remove()
        {
            var repository = new CacheRepository<string, string>();
            repository.TrySetValue("key1", "value1");
            Assert.Equal(1, repository.Count());
            repository.Remove("key1");
            Assert.Equal(0, repository.Count());
        }
    }
}
