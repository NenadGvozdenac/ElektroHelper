using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class UpvotePostRepository : IUpvotePostRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;

    public UpvotePostRepository(IGraphDatabaseContext graphDatabaseContext)
    {
        _graphDatabaseContext = graphDatabaseContext;
    }

    public async Task<bool> AddUpvoteToPostAsync(Guid postId, User user)
    {
        var upvotedAt = DateTime.UtcNow;

        var query = @"
            MATCH (p:Post {id: $postId})
            MATCH (u:User {id: $userId})
            MERGE (u)-[r:UPVOTED_POST]->(p)
            ON CREATE SET r.upvotedAt = $upvotedAt
            RETURN COUNT(r) > 0";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", user.Id },
            { "upvotedAt", upvotedAt.ToNeo4jDateTime() }
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

    public async Task<bool> RemoveUpvoteFromPostAsync(Guid postId, User user)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED_POST]->(p:Post {id: $postId})
            DELETE r";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", user.Id }
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

    public async Task<bool> UserUpvotedPostAsync(Guid postId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED_POST]->(p:Post {id: $postId})
            RETURN COUNT(r) > 0";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", id }
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

    public async Task<bool> RemoveUpvoteFromPostIfExistsAsync(Guid postId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED_POST]->(p:Post {id: $postId})
            DELETE r";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", id }
        };

        try {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            return true;
        } catch {
            return false;
        }
    }
}