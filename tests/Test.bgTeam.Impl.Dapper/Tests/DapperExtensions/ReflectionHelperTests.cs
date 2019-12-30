using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.DapperExtensions
{
    public class ReflectionHelperTests
    {
        [Fact]
        public void GetPropertyShouldReturnNullForNotLambdaConvertAndMemberAccessNodeType()
        {
            ParameterExpression paramExpr = Expression.Parameter(typeof(int), "sum");
            LambdaExpression lambdaExpr = Expression.Lambda(
                Expression.Lambda(
                    Expression.Add(
                        paramExpr,
                        Expression.Constant(1)
                    )),
                new List<ParameterExpression>() { paramExpr }
            );
            var memberInfo = ReflectionHelper.GetProperty(lambdaExpr);
            Assert.Null(memberInfo);
        }

        /*[Fact]
        public void GetProperty()
        {
            ParameterExpression paramExpr = Expression.Parameter(typeof(int?), "Id");
            var property = typeof(TestEntity).GetProperty("Id");
            var propertyAccess = Expression.MakeMemberAccess(paramExpr, property);
            var lambda = Expression.Lambda(propertyAccess);
            var memberInfo = ReflectionHelper.GetProperty(lambda);
            Assert.Null(memberInfo);
        }*/
    }
}
