using bgTeam.Core.Tests.Infrastructure;
using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Dapper.Mapper
{
    public class PluralizedAutoClassMapperTests
    {
        [Fact]
        public void ReplaceWhenRegexWasMatched()
        {
            var mapper = new PluralizedAutoClassMapper<TestClass>();
            Assert.Equal("TestClasses", mapper.TableName);
        }

        [Fact]
        public void ReplaceWhenRegexWasExactMatched()
        {
            var mapper = new PluralizedAutoClassMapper<Person>();
            Assert.Equal("People", mapper.TableName);
        }

        [Fact]
        public void ShouldNotPluralizeIfFoundedInExclusions()
        {
            var mapper = new PluralizedAutoClassMapper<Equipment>();
            Assert.Equal("Equipment", mapper.TableName);
        }

        class TestClass
        {
        }

        class Person
        {
        }

        class Equipment
        {
        }
    }
}
