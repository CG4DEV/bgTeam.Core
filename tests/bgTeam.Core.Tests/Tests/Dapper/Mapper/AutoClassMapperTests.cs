using bgTeam.DataAccess.Impl.Dapper;
using DapperExtensions.Mapper;
using System.Linq;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Dapper.Mapper
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

        [Fact]
        public void Schema()
        {
            var mapper = new AutoClassMapper<TestClass>();
            Assert.Equal("pb", mapper.SchemaName);

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
            [Identity]
            public int Id { get; set; }

            [Ignore]
            public string Name { get; set; }

            [MapTo("some_column")]
            public string SomeColumn { get; set; }
        }

        public class TestClass2
        {
        }
    }
}
