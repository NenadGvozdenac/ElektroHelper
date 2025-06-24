using System.Linq.Expressions;
using MongoDB.Driver;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;

namespace payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

public class MongoDatabaseContext : IDocumentDatabaseContext
{
    private readonly IMongoDatabase _database;

    public MongoDatabaseContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        _database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
    }

    public Task AddDocument<T>(string collectionName, T document) where T : BaseEntity
    {
        var collection = _database.GetCollection<T>(collectionName);
        return collection.InsertOneAsync(document);
    }

    public Task<IQueryable<T>> GetCollection<T>(string collectionName) where T : BaseEntity
    {
        var collection = _database.GetCollection<T>(collectionName);
        return Task.FromResult(collection.AsQueryable());
    }

    public Task<IQueryable<T>> GetCollection<T>(string collectionName, int pageNumber, int pageSize) where T : BaseEntity
    {
        var collection = _database.GetCollection<T>(collectionName);
        var queryableCollection = collection.AsQueryable();
        return Task.FromResult(queryableCollection.Skip((pageNumber - 1) * pageSize).Take(pageSize));
    }

    public Task<IQueryable<T>> GetCollection<T>(string collectionName, int pageNumber, int pageSize, Expression<Func<T, bool>> filter) where T : BaseEntity
    {
        var collection = _database.GetCollection<T>(collectionName);
        var queryableCollection = collection.AsQueryable().Where(filter);
        return Task.FromResult(queryableCollection.Skip((pageNumber - 1) * pageSize).Take(pageSize));
    }

    public Task<T> GetDocumentById<T>(string collectionName, string id) where T : BaseEntity
    {
        var collection = _database.GetCollection<T>(collectionName);
        return collection.Find(document => document.Id == id).FirstOrDefaultAsync();
    }
}