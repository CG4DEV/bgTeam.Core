using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl
{
    public class QueryBuilderTests
    {
        [Fact]
        public void DependencyQueryFactory()
        {
            var (queryFactory, query) = GetMocks();
            Assert.Throws<ArgumentNullException>("commandFactory", () =>
            {
                new QueryBuilder(null);
            });
        }

        [Fact]
        public void Execute()
        {
            var (queryFactory, query) = GetMocks();
            var queryBuilder = new QueryBuilder(queryFactory.Object);
            queryBuilder.Execute(16);
            query.Verify(x => x.Execute(16));
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var (queryFactory, query) = GetMocks();
            var queryBuilder = new QueryBuilder(queryFactory.Object);
            await queryBuilder.ExecuteAsync(16);
            query.Verify(x => x.ExecuteAsync(16, default));
        }

        [Fact]
        public void Build()
        {
            var (queryFactory, query) = GetMocks();
            var queryBuilder = new QueryBuilder(queryFactory.Object);
            Assert.NotNull(queryBuilder.Build(16));
        }

        [Fact]
        public async Task ExecuteAsyncWithQueryReturn()
        {
            var (queryFactory, _) = GetMocks();
            var queryBuilder = new QueryBuilder(queryFactory.Object);

            var query = new Mock<IQuery<int, int>>();
            queryFactory.Setup(x => x.Create<int, int>())
                .Returns(query.Object);

            await queryBuilder.ExecuteAsync<int, int>(16);
            query.Verify(x => x.ExecuteAsync(16, default));
        }

        private (Mock<IQueryFactory>, Mock<IQuery<int>>) GetMocks()
        {
            var queryFactory = new Mock<IQueryFactory>();
            var query = new Mock<IQuery<int>>();
            var queryBuilder = new QueryBuilder(queryFactory.Object);
            queryFactory.Setup(x => x.Create<int>())
                .Returns(query.Object);
            return (queryFactory, query);
        }
    }
}
