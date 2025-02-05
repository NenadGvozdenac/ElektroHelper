namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IUserRepository {
    public Task<User?> AddAsync(User user);
}