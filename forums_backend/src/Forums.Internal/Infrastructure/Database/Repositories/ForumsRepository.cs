using AutoMapper;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class ForumsRepository : IForumsRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;
    private readonly IMapper _mapper;

    public ForumsRepository(IGraphDatabaseContext graphDatabaseContext, IMapper mapper)
    {
        _graphDatabaseContext = graphDatabaseContext;
        _mapper = mapper;
    }

    public async Task<Forum?> AddAsync(Forum entity, User user)
    {
        var query = @"
            MERGE (u:User { id: $userId })
            ON CREATE SET u.email = $userEmail, u.username = $username, u.role = $userRole
            CREATE (forum:Forum {
                id: $id,
                name: $name,
                description: $description,
                createdAt: $createdAt,
                isDeleted: false,
                isQuarantined: false
            })
            CREATE (u)-[:CREATED]->(forum)
            RETURN forum
        ";

        var parameters = new Dictionary<string, object>
        {
            { "id", entity.Id.ToString() },
            { "name", entity.Name },
            { "description", entity.Description },
            { "userId", user.Id },
            { "userEmail", user.Email },
            { "username", user.Username },
            { "userRole", user.Role },
            { "createdAt", entity.CreatedAt.ToNeo4jDateTime() }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();
            return _mapper.Map<Forum>(result["forum"].As<INode>());
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<Forum>> GetAllAsync()
    {
        var query = @"
            MATCH (forum:Forum)
            RETURN forum";

        var resultCursor = await _graphDatabaseContext.RunAsync(query);

        var nodes = await resultCursor.ToListAsync(record => record["forum"].As<INode>());
        return _mapper.Map<IEnumerable<Forum>>(nodes);
    }

    public async Task<Forum?> GetByIdAsync(Guid id)
    {
        var query = @"
            MATCH (forum:Forum { id: $id })
            RETURN forum";

        var parameters = new Dictionary<string, object> { { "id", id.ToString() } };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();
            return _mapper.Map<Forum>(result["forum"].As<INode>());
        }
        catch
        {
            return null;
        }
    }
}
