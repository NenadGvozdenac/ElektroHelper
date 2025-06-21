namespace payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

public interface IDocumentDatabaseContext
{
    public Task<IQueryable<T>> GetCollection<T>(string collectionName) where T : class;
    public Task<T> GetDocumentById<T>(string collectionName, string id) where T : class;
    public Task AddDocument<T>(string collectionName, T document) where T : class;
}