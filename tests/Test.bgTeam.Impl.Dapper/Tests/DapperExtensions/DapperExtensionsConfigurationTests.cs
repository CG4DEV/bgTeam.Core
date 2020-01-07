using DapperExtensions;
using DapperExtensions.Mapper;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.DapperExtensions
{
    public class DapperExtensionsConfigurationTests
    {
        [Fact]
        public void GetNextGuid()
        {
            var dapperExtensionsConfiguration = new DapperExtensionsConfiguration();
            var guid1 = dapperExtensionsConfiguration.GetNextGuid();
            var guid2 = dapperExtensionsConfiguration.GetNextGuid();
            Assert.NotEqual(guid1, guid2);
        }

        [Fact]
        public void ClearCache()
        {
            var dapperExtensionsConfiguration = new DapperExtensionsConfiguration();
            dapperExtensionsConfiguration.GetMap<TestEntity>();
            var propertyInfo = typeof(DapperExtensionsConfiguration).GetField("_classMaps", BindingFlags.Instance | BindingFlags.NonPublic);
            var cache = (ConcurrentDictionary<Type, IClassMapper>)propertyInfo.GetValue(dapperExtensionsConfiguration);
            Assert.NotEmpty(cache);
            dapperExtensionsConfiguration.ClearCache(); 
            cache = (ConcurrentDictionary<Type, IClassMapper>)propertyInfo.GetValue(dapperExtensionsConfiguration);
            Assert.Empty(cache);
        }
    }
}
