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
    public class QueryReturnTests
    {
        [Fact]
        public void Execute()
        {
            var (queryFactory, query) = GetMocks();
            var queryBuilder = new QueryBuilder(queryFactory.Object);
            var queryReturn = queryBuilder.Build(16);
            queryReturn.Execute();
            query.Verify(x => x.Execute(16));
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var (queryFactory, query) = GetMocks();
            var queryBuilder = new QueryBuilder(queryFactory.Object);
            var queryReturn = queryBuilder.Build(16);
            await queryReturn.ExecuteAsync();
            query.Verify(x => x.ExecuteAsync(16, default));
        }

        [Fact]
        public void Return()
        {
            var (queryFactory, _) = GetMocks();
            var query = new Mock<IQuery<int, int>>();
            queryFactory.Setup(x => x.Create<int, int>())
                .Returns(query.Object);
            var queryBuilder = new QueryBuilder(queryFactory.Object);
            var queryReturn = queryBuilder.Build(16);
            queryReturn.Return<int>();
            query.Verify(x => x.Execute(16));
        }

        [Fact]
        public async Task ReturnAsync()
        {
            var (queryFactory, _) = GetMocks();
            var query = new Mock<IQuery<int, int>>();
            queryFactory.Setup(x => x.Create<int, int>())
                .Returns(query.Object);
            var queryBuilder = new QueryBuilder(queryFactory.Object);
            var queryReturn = queryBuilder.Build(16);
            await queryReturn.ReturnAsync<int>();
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
