using bgTeam.DataAccess.Impl.Memory;
using System.Linq;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.Memory
{
    public class MemoryRepositoryTests
    {
        [Fact]
        public void TrySetGetValue()
        {
            var memoryRepository = new MemoryRepository<string, string>();
            Assert.True(memoryRepository.TrySetValue("key1", "value1"));
            Assert.True(memoryRepository.TryGetValue("key1", out string value));
            Assert.Equal("value1", value);
        }

        [Fact]
        public void Count()
        {
            var memoryRepository = new MemoryRepository<string, string>();
            memoryRepository.TrySetValue("key1", "value1");
            memoryRepository.TrySetValue("key2", "value2");
            Assert.Equal(2, memoryRepository.Count());
        }

        [Fact]
        public void GetAll()
        {
            var memoryRepository = new MemoryRepository<string, string>();
            memoryRepository.TrySetValue("key1", "value1");
            memoryRepository.TrySetValue("key2", "value2");
            var all = memoryRepository.GetAll();
            Assert.Equal(2, all.Count());
        }

        [Fact]
        public void Remove()
        {
            var memoryRepository = new MemoryRepository<string, string>();
            memoryRepository.TrySetValue("key1", "value1");
            Assert.Equal(1, memoryRepository.Count());
            memoryRepository.Remove("key1");
            Assert.Equal(0, memoryRepository.Count());
        }
    }
}
