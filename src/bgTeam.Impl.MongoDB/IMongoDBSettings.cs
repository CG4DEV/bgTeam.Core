namespace bgTeam.Impl.MongoDB
{
    /// <summary>
    /// MongoDB connetion settings
    /// </summary>
    public interface IMongoDBSettings
    {
        /// <summary>
        /// Connection string like: mongodb://{Host}:{Port}/{DatabaseName}
        /// </summary>
        string MongoDBConnectionString { get; set; }
    }
}
