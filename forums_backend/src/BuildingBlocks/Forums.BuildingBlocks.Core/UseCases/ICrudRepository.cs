namespace forums_backend.src.BuildingBlocks.Forums.BuildingBlocks.Core.UseCases;

public interface ICrudRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(Guid id);
}