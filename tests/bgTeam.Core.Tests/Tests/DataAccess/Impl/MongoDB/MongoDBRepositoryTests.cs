using System;
using bgTeam.Impl.MongoDB;
using Moq;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.MongoDB
{
    public class MongoDBRepositoryTests
    {
        private readonly Mock<IMongoDBClient> _clientMock;
        private readonly MongoDBRepositoryOnlyForTest _repository;

        public MongoDBRepositoryTests()
        {
            _clientMock = new Mock<IMongoDBClient>();
            _repository = new MongoDBRepositoryOnlyForTest(_clientMock.Object);
        }

        [Theory]
        [InlineData(typeof(String), "String")]
        [InlineData(typeof(GenericType<String>), "GenericType_String")]
        [InlineData(typeof(GenericType<GenericType<String>>), "GenericType_GenericType_String")]
        [InlineData(typeof(GenericType<String, Int32>), "GenericType_String_Int32")]
        [InlineData(typeof(GenericType<GenericType<String, Int32>, GenericType<String, Int32>>), "GenericType_GenericType_String_Int32_GenericType_String_Int32")]
        public void GetCollectionName_FromGenericType_ReturnsCollectionName(Type type, string expected)
        {
            var collectionName = _repository.GetCollectionName(type);
            Assert.Equal(expected, collectionName);
        }

        private class GenericType<T>
        {
            public T Value { get; set; }
        }

        private class GenericType<T1, T2>
        {
            public T1 Value1 { get; set; }

            public T2 Value2 { get; set; }
        }

        private class MongoDBRepositoryOnlyForTest : MongoDBRepository
        {
            public MongoDBRepositoryOnlyForTest(IMongoDBClient client) : base(client)
            {
            }

            public string GetCollectionName(Type type)
            {
                return base.GetCollectionName(type);
            }
        }
    }
}
