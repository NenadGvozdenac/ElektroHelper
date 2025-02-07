using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class DownvotePostRepository : IDownvotePostRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;

    public DownvotePostRepository(IGraphDatabaseContext graphDatabaseContext)
    {
        _graphDatabaseContext = graphDatabaseContext;
    }

    public async Task<bool> AddDownvoteToPostAsync(Guid postId, string userId)
    {
        var query = @"
            MATCH (p:Post {id: $postId})
            MATCH (u:User {id: $userId})
            MERGE (u)-[r:DOWNVOTED_POST]->(p)
            ON CREATE SET r.downvotedAt = $downvotedAt
            RETURN COUNT(r) > 0";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", userId },
            { "downvotedAt", DateTime.UtcNow.ToNeo4jDateTime() }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveDownvoteFromPostAsync(Guid postId, string userId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED_POST]->(p:Post {id: $postId})
            DELETE r";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", userId }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UserDownvotedPostAsync(Guid postId, string userId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED_POST]->(p:Post {id: $postId})
            RETURN COUNT(r) > 0";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", userId }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return bool.Parse(result[0].As<string>());
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveDownvoteFromPostIfExistsAsync(Guid postId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED_POST]->(p:Post {id: $postId})
            DELETE r";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", id }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            return true;
        }
        catch
        {
            return false;
        }
    }
}