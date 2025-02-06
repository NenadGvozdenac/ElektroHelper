using AutoMapper;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;
    private readonly IMapper _mapper;

    public UserRepository(IGraphDatabaseContext graphDatabaseContext, IMapper mapper)
    {
        _graphDatabaseContext = graphDatabaseContext;
        _mapper = mapper;
    }

    public async Task<User?> AddAsync(User user)
    {
        var query = @"
            MERGE (user:User {id: $id})
            ON CREATE SET 
                user.email = $email,
                user.username = $username,
                user.role = $role,
                user.isBanned = false,
                user.reasonForBan = '',
                user.isDeleted = false
            RETURN user
        ";

        var parameters = new Dictionary<string, object>
        {
            { "id", user.Id },
            { "email", user.Email },
            { "username", user.Username },
            { "role", user.Role }
        };

        try {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return _mapper.Map<User>(result["user"].As<INode>());
        } catch {
            return null;
        }
    }
}