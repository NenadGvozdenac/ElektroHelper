using System.Linq.Expressions;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;

namespace payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

public interface IDocumentDatabaseContext
{
    public Task<IQueryable<T>> GetCollection<T>(string collectionName) where T : BaseEntity;
    public Task<IQueryable<T>> GetCollection<T>(string collectionName, int pageNumber, int pageSize) where T : BaseEntity;
    public Task<IQueryable<T>> GetCollection<T>(string collectionName, int pageNumber, int pageSize, Expression<Func<T, bool>> filter) where T : BaseEntity;
    public Task<T> GetDocumentById<T>(string collectionName, string id) where T : BaseEntity;
    public Task AddDocument<T>(string collectionName, T document) where T : BaseEntity;
}