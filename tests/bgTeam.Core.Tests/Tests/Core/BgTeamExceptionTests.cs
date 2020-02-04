using System;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core
{
    public class WebClientExceptionTests
    {
        [Fact]
        public async Task Exception()
        {
            await Assert.ThrowsAsync<BgTeamException>(() => throw new BgTeamException());
        }

        [Fact]
        public async Task ExceptionWithMessage()
        {
            var ex = await Assert.ThrowsAsync<BgTeamException>(() => throw new BgTeamException("msg"));
            Assert.Equal("msg", ex.Message);
        }

        [Fact]
        public async Task ExceptionWithInnerException()
        {
            var ex = await Assert.ThrowsAsync<BgTeamException>(() => throw new BgTeamException("msg", new Exception()));
            Assert.Equal("msg", ex.Message);
            Assert.NotNull(ex.InnerException);
        }
    }
}
