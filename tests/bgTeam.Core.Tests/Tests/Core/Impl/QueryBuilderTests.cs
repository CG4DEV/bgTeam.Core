using bgTeam;
using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Core.Impl
{
    public class QueryBuilderTests
    {
        [Fact]
        public void SingletonsShoulPersist()
        {
            var services = (IServiceCollection)new ServiceCollection();
            var provider = services
                .AddSingleton(services)
                .AddSingleton<IQueryFactory, QueryFactory>()
                .AddSingleton<IQueryBuilder, QueryBuilder>()
                .AddSingleton<ITestSingleton, TestSingleton>()
                .AddTransient<IQuery<int, int>, TestStory>()
                .BuildServiceProvider();

            var builder = provider.GetRequiredService<IQueryBuilder>();

            Assert.Equal(2, builder.Build(2).Return<int>());
            Assert.Equal(4, builder.Build(2).Return<int>());
            Assert.Equal(6, builder.Build(2).Return<int>());
        }

        private class TestStory : IQuery<int, int>
        {
            private readonly ITestSingleton _testSingleton;

            public TestStory(ITestSingleton testSingleton)
            {
                _testSingleton = testSingleton;
            }

            public int Execute(int context)
                => ExecuteAsync(context).GetAwaiter().GetResult();

            public async Task<int> ExecuteAsync(int context, CancellationToken ct = default)
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
