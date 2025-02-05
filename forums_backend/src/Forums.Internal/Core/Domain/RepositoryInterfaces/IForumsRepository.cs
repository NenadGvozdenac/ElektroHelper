using forums_backend.src.Forums.BuildingBlocks.Core.UseCases;

namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IForumsRepository
{
    public Task<Forum?> AddAsync(Forum entity, User user);
    public Task<IEnumerable<Forum>> GetAllAsync();
    public Task<Forum?> GetByIdAsync(Guid id);
}