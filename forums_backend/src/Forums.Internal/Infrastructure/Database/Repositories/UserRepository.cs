using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;

    public UserRepository(IGraphDatabaseContext graphDatabaseContext)
    {
        _graphDatabaseContext = graphDatabaseContext;
    }

    public async Task<User?> AddAsync(User user)
    {
        var query = @"
            MERGE (user:User {id: $id})
            ON CREATE SET 
                user.email = $email,
                user.username = $username,
                user.role = $role,
                user.IsBanned = false,
                user.ReasonForBan = '',
                user.IsDeleted = false
        ";

        var parameters = new Dictionary<string, object>
        {
            { "id", user.Id },
            { "email", user.Email },
            { "username", user.Username },
            { "role", user.Role }
        };

        try {
            await _graphDatabaseContext.RunAsync(query, parameters);
            return user;
        } catch {
            return null;
        }
    }
}