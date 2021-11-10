using System.Threading;
using System.Threading.Tasks;
using bgTeam.Impl;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace bgTeam.Core.Tests.Core.Impl
{
    public class StoryBuilderTests
    {
        [Fact]
        public void SingletonsShoulPersist()
        {
            var services = (IServiceCollection)new ServiceCollection();
            var provider = services
                .AddSingleton(services)
                .AddSingleton<IStoryFactory, StoryFactory>()
                .AddSingleton<IStoryBuilder, StoryBuilder>()
                .AddSingleton<ITestSingleton, TestSingleton>()
                .AddTransient<IStory<int, int>, TestStory>()
                .BuildServiceProvider();

            var builder = provider.GetRequiredService<IStoryBuilder>();

            Assert.Equal(2, builder.Build(2).ReturnAsync<int>().Result);
            Assert.Equal(4, builder.Build(2).ReturnAsync<int>().Result);
            Assert.Equal(6, builder.Build(2).ReturnAsync<int>().Result);
        }

        private class TestStory : IStory<int, int>
        {
            private readonly ITestSingleton _testSingleton;

            public TestStory(ITestSingleton testSingleton)
            {
                _testSingleton = testSingleton;
            }

            public int Execute(int context)
                => ExecuteAsync(context).GetAwaiter().GetResult();

            public async Task<int> ExecuteAsync(int context)
            {
                await Task.Yield();

                return context * _testSingleton.Increment();
            }
        }

        private class TestSingleton : ITestSingleton
        {
            private int m_value;

            public int Increment() => Interlocked.Increment(ref m_value);
        }

        private interface ITestSingleton
        {
            int Increment();
        }
    }
}
