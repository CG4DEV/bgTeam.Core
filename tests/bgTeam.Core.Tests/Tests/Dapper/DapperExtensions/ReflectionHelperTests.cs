using bgTeam.Core.Tests.Infrastructure;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace bgTeam.Core.Tests.Dapper.DapperExtensions
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

        [Fact]
        public void GetProperty()
        {
            ParameterExpression x = Expression.Parameter(typeof(TestEntity), "x");
            PropertyInfo isIdAccessor = typeof(TestEntity).GetProperty("Id");
            MemberExpression getIsId = Expression.MakeMemberAccess(x, isIdAccessor);
            var lambda = Expression.Lambda(getIsId, x);
            var memberInfo = ReflectionHelper.GetProperty(lambda);
            Assert.NotNull(memberInfo);
            Assert.Equal(MemberTypes.Property, memberInfo.MemberType);
        }

        [Fact]
        public void IsSimpleType()
        {
            Assert.True(ReflectionHelper.IsSimpleType(typeof(byte?)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(byte)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(sbyte)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(short)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(ushort)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(int)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(uint)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(long)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(ulong)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(float)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(double)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(decimal)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(bool)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(string)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(char)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(Guid)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(DateTime)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(DateTimeOffset)));
            Assert.True(ReflectionHelper.IsSimpleType(typeof(byte[])));
            Assert.False(ReflectionHelper.IsSimpleType(typeof(TestEntity)));
        }

        [Fact]
        public void GetObjectValues()
        {
            var values = ReflectionHelper.GetObjectValues(new TestEntity { Id = 15, Name = "Alex" });
            Assert.Equal(15, values["Id"]);
            Assert.Equal("Alex", values["Name"]);
            values = ReflectionHelper.GetObjectValues(new TestClass());
            Assert.Empty(values);
            values = ReflectionHelper.GetObjectValues(null);
            Assert.Empty(values);
        }

        class TestClass
        {
        }
    }
}
