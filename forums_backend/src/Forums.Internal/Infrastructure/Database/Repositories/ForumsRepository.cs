using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class ForumsRepository : IForumsRepository
{
    public Task<Forum> AddAsync(Forum entity)
    {
        throw new NotImplementedException();
    }

    public Task<Forum> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Forum>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Forum> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Forum> UpdateAsync(Forum entity)
    {
        throw new NotImplementedException();
    }
}