using MongoDB.Driver;

namespace payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

public class MongoDatabaseContext : IDocumentDatabaseContext
{
    private readonly IMongoDatabase _database;

    public MongoDatabaseContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        _database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
    }

    public Task AddDocument<T>(string collectionName, T document) where T : class
    {
        var collection = _database.GetCollection<T>(collectionName);
        return collection.InsertOneAsync(document);
    }

    public Task<IQueryable<T>> GetCollection<T>(string collectionName) where T : class
    {
        var collection = _database.GetCollection<T>(collectionName);
        return Task.FromResult(collection.AsQueryable());
    }

    public Task<T> GetDocumentById<T>(string collectionName, string id) where T : class
    {
        var collection = _database.GetCollection<T>(collectionName);
        return collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
    }
}