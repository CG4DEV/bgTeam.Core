namespace bgTeam.Impl.MongoDB
{
    using global::MongoDB.Driver;

    public class MongoDBClient : IMongoDBClient
    {
        private readonly MongoUrl _mongoUrl;
        private readonly IMongoClient _mongoClient;

        public MongoDBClient(IMongoDBSettings settings)
        {
            _mongoUrl = MongoUrl.Create(settings.MongoDBConnectionString);
            _mongoClient = new MongoClient(_mongoUrl);
        }

        public IMongoDatabase GetDatabase()
        {
            return _mongoClient.GetDatabase(_mongoUrl.DatabaseName);
        }
    }
}
