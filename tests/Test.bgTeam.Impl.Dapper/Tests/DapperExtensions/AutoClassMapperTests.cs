using bgTeam.DataAccess.Impl.Dapper;
using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests.DapperExtensions
{
    public class AutoClassMapperTests
    {
        [Fact]
        public void Table()
        {
            var mapper = new AutoClassMapper<TestClass>();
            Assert.Equal("test_class", mapper.TableName);

            var mapper2 = new AutoClassMapper<TestClass2>();
            Assert.Equal("TestClass2", mapper2.TableName);
        }

        [Fact(Skip = "Work not as planning?")]
        public void Schema()
        {
            var mapper = new AutoClassMapper<TestClass>();
            Assert.Equal("public", mapper.SchemaName);

            var mapper2 = new AutoClassMapper<TestClass2>();
            Assert.Null(mapper2.SchemaName);
        }

        [Fact]
        public void ColumnPrefix()
        {
            var mapper = new AutoClassMapper<TestClass>();
            Assert.Equal("b_Id", mapper.Properties.First().ColumnName);
        }

        [PrefixForColumns("b_")]
        [TableName("test_class")]
        [Schema("pb")]
        class TestClass
        { 
            public int Id { get; set; }

            [Ignore]
            public string Name { get; set; }
        }

        class TestClass2
        {
        }
    }
}
