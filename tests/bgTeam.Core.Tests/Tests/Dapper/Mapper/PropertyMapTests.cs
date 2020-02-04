using DapperExtensions.Mapper;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Dapper.Mapper
{
    public class PropertyMapTests
    {
        [Fact]
        public void FieldShouldNotBeUsedAsAPrimaryKeyIfItIgnored()
        {
            var propertyMap = new PropertyMap(typeof(TestClass).GetProperty("Id"));
            propertyMap.Ignore();
            Assert.Throws<ArgumentException>(() => propertyMap.Key(KeyType.PrimaryKey));
        }

        [Fact]
        public void FieldShouldNotBeUsedAsAPrimaryKeyIfItIsReadOnly()
        {
            var propertyMap = new PropertyMap(typeof(TestClass).GetProperty("Id"));
            propertyMap.ReadOnly();
            Assert.Throws<ArgumentException>(() => propertyMap.Key(KeyType.PrimaryKey));
        }

        [Fact]
        public void FieldShouldNotBeIgnoredIfItIsPrimaryKey()
        {
            var propertyMap = new PropertyMap(typeof(TestClass).GetProperty("Id"));
            propertyMap.Key(KeyType.Guid);
            Assert.Throws<ArgumentException>(() => propertyMap.Ignore());
        }

        [Fact]
        public void FieldShouldNotBeReadOnlyIfItIsPrimaryKey()
        {
            var propertyMap = new PropertyMap(typeof(TestClass).GetProperty("Id"));
            propertyMap.Key(KeyType.Guid);
            Assert.Throws<ArgumentException>(() => propertyMap.ReadOnly());
        }
    }

    class TestClass
    {
        public int Id { get; set; }
    }
}
