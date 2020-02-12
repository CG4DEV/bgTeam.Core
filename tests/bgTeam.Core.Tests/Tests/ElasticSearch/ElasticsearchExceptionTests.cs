using bgTeam.ElasticSearch;
using System;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.ElasticSearch
{
    public class ElasticsearchExceptionTests
    {
        [Fact]
        public async Task Exception()
        {
            await Assert.ThrowsAsync<ElasticsearchException>(() => throw new ElasticsearchException());
        }

        [Fact]
        public async Task ExceptionWithMessage()
        {
            var ex = await Assert.ThrowsAsync<ElasticsearchException>(() => throw new ElasticsearchException("msg"));
            Assert.Equal("msg", ex.Message);
        }

        [Fact]
        public async Task ExceptionWithInnerException()
        {
            var ex = await Assert.ThrowsAsync<ElasticsearchException>(() => throw new ElasticsearchException("msg", new Exception()));
            Assert.Equal("msg", ex.Message);
            Assert.NotNull(ex.InnerException);
        }
    }
}
