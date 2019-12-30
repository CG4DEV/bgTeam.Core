using bgTeam;
using bgTeam.Impl;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Test.bgTeam.Core
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

            Assert.Equal(2, builder.Build(2).Return<int>());
            Assert.Equal(4, builder.Build(2).Return<int>());
            Assert.Equal(6, builder.Build(2).Return<int>());
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
