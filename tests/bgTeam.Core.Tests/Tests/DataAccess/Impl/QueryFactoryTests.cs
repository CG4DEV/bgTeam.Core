using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl;
using Moq;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl
{
    public class QueryFactoryTests
    {
        [Fact]
        public void DependencyServiceProvider()
        {
            var serviceProvider = GetServiceProvider();
            Assert.Throws<ArgumentNullException>("provider", () =>
            {
                new QueryFactory(null);
            });
        }

        [Fact]
        public void Create()
        {
            var serviceProvider = GetServiceProvider();
            serviceProvider.Setup(x => x.GetService(It.IsAny<Type>()))
                .Returns(new Mock<IQuery<int>>().Object);
            var queryFactory = new QueryFactory(serviceProvider.Object);
            var service = queryFactory.Create<int>();
            Assert.NotNull(service);
        }

        [Fact]
        public void CreateWithTResult()
        {
            var serviceProvider = GetServiceProvider();
            serviceProvider.Setup(x => x.GetService(It.IsAny<Type>()))
                .Returns(new Mock<IQuery<int, int>>().Object);
            var queryFactory = new QueryFactory(serviceProvider.Object);
            var service = queryFactory.Create<int, int>();
            Assert.NotNull(service);
        }


        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            return serviceProvider;
        }
    }
}
