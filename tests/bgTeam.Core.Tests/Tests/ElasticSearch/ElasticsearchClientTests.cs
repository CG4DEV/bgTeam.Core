using bgTeam.Core.Tests.Infrastructure;
using bgTeam.ElasticSearch;
using bgTeam.Impl.ElasticSearch;
using Elasticsearch.Net;
using Moq;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.ElasticSearch
{
    public class ElasticsearchClientTests
    {
        [Fact]
        public void DependencyElasticsearchConnectionFactory()
        {
            Assert.Throws<ArgumentNullException>("connectionFactoryEs", () =>
            {
                new ElasticsearchClient(null);
            });
        }

        [Fact]
        public void SuccessfulGetShouldReturnEntity()
        {
            var (factory, client) = GetMocks();
            var response = SuccessfulGetResponse(new TestEntity());
            client.Setup(x => x.Get(It.IsAny<DocumentPath<TestEntity>>(), null))
                .Returns(response);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            Assert.NotNull(elasticSearchClient.Get<TestEntity>("id", "index"));
        }

        [Fact]
        public void UnsuccessfulGetShouldThrowExceptionIfFailureReasonIsNotBadResponseAndReasonIsNot404StatusCode()
        {
            var (factory, client) = GetMocks();
            var response = ExceptionGetResponse<TestEntity>(new ElasticsearchClientException("Smth went wrong"));
            client.Setup(x => x.Get(It.IsAny<DocumentPath<TestEntity>>(), null))
                .Returns(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            Assert.Throws<ElasticsearchException>(() => elasticSearchClient.Get<TestEntity>("id", "index"));
        }

        [Fact]
        public void UnsuccessfulGetShouldReturnsNullIfFailureReasonIsBadResponseOrReasonIs404StatusCode()
        {
            var (factory, client) = GetMocks();
            var response = ExceptionGetResponse<TestEntity>(new ElasticsearchClientException(PipelineFailure.BadResponse, "Not correct index name", new Exception()));
            client.Setup(x => x.Get(It.IsAny<DocumentPath<TestEntity>>(), null))
                .Returns(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            Assert.Null(elasticSearchClient.Get<TestEntity>("id", "index"));
        }

        [Fact]
        public async Task SuccessfulGetAsyncShouldReturnEntity()
        {
            var (factory, client) = GetMocks();
            var response = SuccessfulGetResponse(new TestEntity());
            client.Setup(x => x.GetAsync(It.IsAny<DocumentPath<TestEntity>>(), null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            var result = await elasticSearchClient.GetAsync<TestEntity>("id", "index");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UnsuccessfulGetAsyncShouldThrowException()
        {
            var (factory, client) = GetMocks();
            var response = ExceptionGetResponse<TestEntity>(new ElasticsearchClientException("Smth went wrong"));
            client.Setup(x => x.GetAsync(It.IsAny<DocumentPath<TestEntity>>(), null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            await Assert.ThrowsAsync<ElasticsearchException>(() => elasticSearchClient.GetAsync<TestEntity>("id", "index"));
        }

        [Fact]
        public async Task UnsuccessfulGetAsyncShouldReturnsNullIfFailureReasonIsBadResponseOrReasonIs404StatusCode()
        {
            var (factory, client) = GetMocks();
            var response = ExceptionGetResponse<TestEntity>(new ElasticsearchClientException(PipelineFailure.BadResponse, "Not correct index name", new Exception()));
            client.Setup(x => x.GetAsync(It.IsAny<DocumentPath<TestEntity>>(), null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            Assert.Null(await elasticSearchClient.GetAsync<TestEntity>("id", "index"));
        }

        [Fact]
        public void Index()
        {
            var (factory, client) = GetMocks();
            var response = new Mock<IndexResponse>();
            response.SetupGet(x => x.IsValid)
                .Returns(true);
            client.Setup(x => x.Index(It.IsAny<TestEntity>(), It.IsAny<Func<IndexDescriptor<TestEntity>, IIndexRequest<TestEntity>>>()))
                .Returns(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            elasticSearchClient.Index(new TestEntity(), "index1");
            client.Verify(x => x.Index(It.IsAny<TestEntity>(), It.IsAny<Func<IndexDescriptor<TestEntity>, IIndexRequest<TestEntity>>>()));
        }

        [Fact]
        public void UnsuccessfulIndexShouldThrowException()
        {
            var (factory, client) = GetMocks();
            var response = new Mock<IndexResponse>();
            response.SetupGet(x => x.IsValid)
                .Returns(false);
            client.Setup(x => x.Index(It.IsAny<TestEntity>(), It.IsAny<Func<IndexDescriptor<TestEntity>, IIndexRequest<TestEntity>>>()))
                .Returns(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            Assert.Throws<ElasticsearchException>(() => elasticSearchClient.Index(new TestEntity(), "index1"));
        }

        [Fact]
        public async Task IndexAsync()
        {
            var (factory, client) = GetMocks();
            var response = new Mock<IndexResponse>();
            response.SetupGet(x => x.IsValid)
                .Returns(true);
            client.Setup(x => x.IndexAsync(It.IsAny<TestEntity>(), It.IsAny<Func<IndexDescriptor<TestEntity>, IIndexRequest<TestEntity>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            await elasticSearchClient.IndexAsync(new TestEntity(), "index1");
            client.Verify(x => x.IndexAsync(It.IsAny<TestEntity>(), It.IsAny<Func<IndexDescriptor<TestEntity>, IIndexRequest<TestEntity>>>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task UnsuccessfulIndexAsyncShouldThrowException()
        {
            var (factory, client) = GetMocks();
            var response = new Mock<IndexResponse>();
            response.SetupGet(x => x.IsValid)
                .Returns(false);
            client.Setup(x => x.IndexAsync(It.IsAny<TestEntity>(), It.IsAny<Func<IndexDescriptor<TestEntity>, IIndexRequest<TestEntity>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            await Assert.ThrowsAsync<ElasticsearchException>(() => elasticSearchClient.IndexAsync(new TestEntity(), "index1"));
        }

        [Fact]
        public void SearchForDateTime()
        {
            var (factory, client) = GetMocks();
            var response = SuccessfulSearchResponse(new ReadOnlyCollection<TestEntity>(new List<TestEntity> { new TestEntity() }));
            client.Setup(x => x.Search<TestEntity>(It.IsAny<ISearchRequest>()))
                .Returns(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            var result = elasticSearchClient.Search<TestEntity>("CreatedAt", DateTime.UtcNow, "index");
            Assert.Single(result);
        }

        [Fact]
        public void SearchForDouble()
        {
            var (factory, client) = GetMocks();
            var response = SuccessfulSearchResponse(new ReadOnlyCollection<TestEntity>(new List<TestEntity> { new TestEntity() }));
            client.Setup(x => x.Search<TestEntity>(It.IsAny<ISearchRequest>()))
                .Returns(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            var result = elasticSearchClient.Search<TestEntity>("Weight", 51.2, "index");
            Assert.Single(result);
        }

        [Fact]
        public void UnsuccessfulSearchShouldThrowException()
        {
            var (factory, client) = GetMocks();
            var response = UnsuccessfulSearchResponse<TestEntity>("smth went wrong");
            client.Setup(x => x.Search<TestEntity>(It.IsAny<ISearchRequest>()))
                .Returns(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            Assert.Throws<ElasticsearchException>(() => elasticSearchClient.Search<TestEntity>("Name", "John", "index"));
        }

        [Fact]
        public async Task SearchAsyncForDateTime()
        {
            var (factory, client) = GetMocks();
            var response = SuccessfulSearchResponse(new ReadOnlyCollection<TestEntity>(new List<TestEntity> { new TestEntity() }));
            client.Setup(x => x.SearchAsync<TestEntity>(It.IsAny<ISearchRequest>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            var result = await elasticSearchClient.SearchAsync<TestEntity>("CreatedAt", DateTime.UtcNow, "index");
            Assert.Single(result);
        }

        [Fact]
        public async Task SearchAsyncForDouble()
        {
            var (factory, client) = GetMocks();
            var response = SuccessfulSearchResponse(new ReadOnlyCollection<TestEntity>(new List<TestEntity> { new TestEntity() }));
            client.Setup(x => x.SearchAsync<TestEntity>(It.IsAny<ISearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            var result = await elasticSearchClient.SearchAsync<TestEntity>("Weight", 51.2, "index");
            Assert.Single(result);
        }

        [Fact]
        public async Task UnsuccessfulSearchAsyncShouldThrowException()
        {
            var (factory, client) = GetMocks();
            var response = UnsuccessfulSearchResponse<TestEntity>("smth went wrong");
            client.Setup(x => x.SearchAsync<TestEntity>(It.IsAny<ISearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response.Object);
            var elasticSearchClient = new ElasticsearchClient(factory.Object);
            await Assert.ThrowsAsync<ElasticsearchException>(() => elasticSearchClient.SearchAsync<TestEntity>("Name", "John", "index"));
        }

        private Mock<ISearchResponse<T>> SuccessfulSearchResponse<T>(IReadOnlyCollection<T> documents)
            where T : class
        {
            var response = new Mock<ISearchResponse<T>>();
            response.SetupGet(x => x.Documents)
                .Returns(documents);
            response.SetupGet(x => x.IsValid)
                .Returns(true);
            return response;
        }

        private Mock<ISearchResponse<T>> UnsuccessfulSearchResponse<T>(string debugInformation)
            where T : class
        {
            var response = new Mock<ISearchResponse<T>>();
            response.SetupGet(x => x.DebugInformation)
                .Returns(debugInformation);
            response.SetupGet(x => x.IsValid)
                .Returns(false);
            return response;
        }

        private GetResponse<T> SuccessfulGetResponse<T>(T source) 
            where T : class
        {
            var getResponse = new GetResponse<T>();

            var sourceInfo = getResponse.GetType().GetProperty(nameof(getResponse.Source));
            sourceInfo.SetValue(getResponse, source);

            var apiCallDetails = new ApiCallDetails { HttpStatusCode = 200, Success = true };
            var _originalApiInfo = typeof(ResponseBase).GetField("_originalApiCall", BindingFlags.NonPublic | BindingFlags.Instance);
            _originalApiInfo.SetValue(getResponse, apiCallDetails);

            return getResponse;
        }

        private Mock<GetResponse<T>> ExceptionGetResponse<T>(ElasticsearchClientException originalException)
            where T : class
        {
            var response = new Mock<GetResponse<T>>();
            var apiCall = new Mock<IApiCallDetails>();
            apiCall.Setup(x => x.OriginalException)
                .Returns(originalException);
            response.SetupGet(x => x.ApiCall)
                .Returns(apiCall.Object);
            response.SetupGet(x => x.IsValid)
                .Returns(false);
            return response;
        }

        private (Mock<IElasticsearchConnectionFactory>, Mock<IElasticClient>) GetMocks()
        {
            var elasticSearchConnectionFactory = new Mock<IElasticsearchConnectionFactory>();
            var elasticSearchClient = new Mock<IElasticClient>();
            elasticSearchConnectionFactory.Setup(x => x.CreateClient())
                .Returns(elasticSearchClient.Object);
            elasticSearchConnectionFactory.Setup(x => x.CreateClientAsync())
                .ReturnsAsync(elasticSearchClient.Object);
            return (elasticSearchConnectionFactory, elasticSearchClient);
        }
    }
}
