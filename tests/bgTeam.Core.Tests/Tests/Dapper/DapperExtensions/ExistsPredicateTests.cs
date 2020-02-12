using bgTeam.Core.Tests.Infrastructure;
using DapperExtensions;
using DapperExtensions.Mapper.Sql;
using System.Collections.Generic;
using Xunit;
using Moq;
using System;

namespace bgTeam.Core.Tests.Dapper.DapperExtensions
{
    public class ExistsPredicateTests
    {
        [Fact]
        public void GetSqlShouldThrowsExceptionIfMapperIsNull()
        {
            var predicate = new ExistsPredicate<TestEntity>();
            var dictionary = new Dictionary<string, object>();

            var sqlGenerator = new Mock<ISqlGenerator>();
            sqlGenerator.SetupGet(x => x.Configuration)
                .Returns(new Mock<IDapperExtensionsConfiguration>().Object);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var sql = predicate.GetSql(sqlGenerator.Object, dictionary);
            });
        }

        [Fact]
        public void GetSql()
        {
            var predicate = new ExistsPredicate<TestEntity>()
            {
                Predicate = new FieldPredicate<TestEntity>()
                { 
                    PropertyName = "Id",
                    Value = 2,
                    Operator = Operator.Eq,
                }
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(EXISTS (SELECT 1 FROM \"TestEntity\" WHERE (Id = @Id_0)))", sql);
            Assert.Equal(2, dictionary["@Id_0"]);
        }

        [Fact]
        public void GetSqlWhenValueIsNull()
        {
            var predicate = new ExistsPredicate<TestEntity>()
            {
                Predicate = new FieldPredicate<TestEntity>()
                {
                    PropertyName = "Id",
                    Value = 2,
                    Operator = Operator.Eq,
                },
                Not = true,
            };
            var dictionary = new Dictionary<string, object>();
            var sql = predicate.GetSql(SqlHelper.GetSqlGenerator(), dictionary);
            Assert.Equal("(NOT EXISTS (SELECT 1 FROM \"TestEntity\" WHERE (Id = @Id_0)))", sql);
            Assert.Equal(2, dictionary["@Id_0"]);
        }
    }
}
