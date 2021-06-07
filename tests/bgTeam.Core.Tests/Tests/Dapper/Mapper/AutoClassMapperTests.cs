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

        [Fact]
        public void CompositePrimaryKey()
        {
            var mapper = new AutoClassMapper<TestClass3>();
            Assert.Equal("key_1", mapper.Properties[0].ColumnName);
            Assert.Equal(KeyType.PrimaryKey, mapper.Properties[0].KeyType);
            Assert.Equal("key_2", mapper.Properties[1].ColumnName);
            Assert.Equal(KeyType.PrimaryKey, mapper.Properties[1].KeyType);
        
            Assert.Equal(2, mapper.Properties.Count(p => p.KeyType == KeyType.PrimaryKey));
        }

        [Fact]
        public void SinglePrimaryKey()
        {
            var mapper = new AutoClassMapper<TestClass4>();
            Assert.Equal("key_1", mapper.Properties[0].ColumnName);
            Assert.Equal(KeyType.PrimaryKey, mapper.Properties[0].KeyType);

            Assert.Equal(1, mapper.Properties.Count(p => p.KeyType == KeyType.PrimaryKey));
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

            [ColumnName("some_column")]
            public string SomeColumn { get; set; }
        }

        public class TestClass2
        {
        }

        class TestClass3
        {
            [PrimaryKey]
            [ColumnName("key_1")]
            public int Key1 { get; set; }

            [PrimaryKey]
            [ColumnName("key_2")]
            public long Key2 { get; set; }

            [ColumnName("some_column")]
            public string SomeColumn { get; set; }
        }

        class TestClass4
        {
            [PrimaryKey]
            [ColumnName("key_1")]
            public int Key1 { get; set; }

            [ColumnName("some_column")]
            public string SomeColumn { get; set; }
        }
    }
}