using DapperExtensions;
using Xunit;
using Moq;
using System.Data;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;

namespace bgTeam.Core.Tests.Tests.Dapper.Builder
{
    [Collection("SqlLiteCollection")]
    public class PredicateConverterTests
    {
        //TODO - finish writting tests, whrn will be fixed
        [Fact]
        public void GeneratingPredicateFromExpression()
        {
            Expression<Func<TestEntity, bool>> sourcePredicate = x => x.Name == "John" &&
                   !(x.Age > 20 || x.IsExists) &&
                   x.IsExists == false &&
                   x.Name.Contains("Hi") &&
                   x.Name == Hi() &&
                   x.Url == new Uri("http://test.com") && 
                   x.Age != 25 &&
                   x.Age <= 30 &&
                   x.Age < 35 &&
                   x.Age >= 18;

            var groupPredicate = GeneratePredicate(sourcePredicate) as PredicateGroup;
            Assert.NotNull(groupPredicate);
            Assert.Equal(10, groupPredicate.Predicates.Count);
            Assert.NotNull(groupPredicate.Predicates[1] as PredicateGroup);
        }

        string Hi()
        {
            return "Hi";
        }

        [Fact]
        public void GeneratingPredicateShouldThrowsExceptionIfOperatorIsNotSupported()
        {
            Assert.Throws<NotImplementedException>(() =>
            {
                GeneratePredicate(x => (4 * x.Age > 6) | (4 < 6));
            });
        }

        IPredicate GeneratePredicate(Expression<Func<TestEntity, bool>> expression)
        {
            IPredicate generatedPredicate = null;
            var dapper = new Mock<IDapperImplementor>();
            dapper.Setup(x => x.GetAsync<TestEntity>(It.IsAny<IDbConnection>(), It.IsAny<IPredicate>(), It.IsAny<IDbTransaction>(), null))
                .Callback((IDbConnection _, IPredicate predicate, IDbTransaction __, int? ___) =>
                {
                    generatedPredicate = predicate;
                })
                .Returns(Task.FromResult<TestEntity>(null));
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            var connection = new Mock<IDbConnection>();
            var entity = connection.Object.Get(expression);
            return generatedPredicate;
        }

        public class TestEntity
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public bool IsExists { get; set; }
            public Uri Url { get; set; }
        }
    }
}
