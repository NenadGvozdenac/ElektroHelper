using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class DownvoteCommentRepository : IDownvoteCommentRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;

    public DownvoteCommentRepository(IGraphDatabaseContext graphDatabaseContext)
    {
        _graphDatabaseContext = graphDatabaseContext;
    }

    public async Task<bool> AddDownvoteToCommentAsync(Guid commentId, string userId)
    {
        var downvotedAt = DateTime.UtcNow;

        var query = @"
            MATCH (c:Comment {id: $commentId})
            MATCH (u:User {id: $userId})
            MERGE (u)-[r:DOWNVOTED]->(c)
            ON CREATE SET r.downvotedAt = $downvotedAt
            RETURN COUNT(r) > 0
        ";

        var parameters = new Dictionary<string, object>
        {
            { "commentId", commentId.ToString() },
            { "userId", userId },
            { "downvotedAt", downvotedAt.ToNeo4jDateTime() }
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

    public async Task<CommentWithDownvotes?> GetCommentWithDownvotesAsync(Guid commentId)
    {
        var query = @"
            MATCH (c:Comment {id: $commentId})
            WITH c
            MATCH (c)<-[r:DOWNVOTED]-(user:User)
            RETURN c, COLLECT({ user: user, downvoteDate: r.downvotedAt }) AS userDownvotes
        ";

        var parameters = new Dictionary<string, object>
        {
            { "commentId", commentId.ToString() }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            var commentNode = result["c"].As<INode>();
            var userDownvotesNodes = result["userDownvotes"].As<List<INode>>();

            var comment = new Comment(
                Guid.Parse(commentNode["id"].As<string>()),
                commentNode["content"].As<string>(),
                commentNode["createdAt"].As<string>().FromNeo4jDateTime()
            );

            var userDownvotes = userDownvotesNodes.Select(userDownvoteNode =>
            {
                var userNode = userDownvoteNode["user"].As<INode>();
                var downvoteDate = userDownvoteNode["downvoteDate"].As<string>().FromNeo4jDateTime();

                var user = new User(
                    userNode["id"].As<string>(),
                    userNode["email"].As<string>(),
                    userNode["role"].As<string>(),
                    userNode["username"].As<string>()
                );

                var userDownvote = new UserDownvote(user, downvoteDate);

                return userDownvote;
            });

            return new CommentWithDownvotes(comment, userDownvotes);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> RemoveDownvoteFromCommentAsync(Guid commentId, string userId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED]->(c:Comment {id: $commentId})
            DELETE r
        ";

        var parameters = new Dictionary<string, object>
        {
            { "commentId", commentId.ToString() },
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

    public async Task<bool> RemoveDownvoteFromCommentIfExistsAsync(Guid commentId, string userId)
    {
        var userDownvotedComment = await UserDownvotedCommentAsync(commentId, userId);

        if (userDownvotedComment)
        {
            var removedUpvote = await RemoveDownvoteFromCommentAsync(commentId, userId);

            return removedUpvote;
        }

        return true;
    }

    public async Task<bool> UserDownvotedCommentAsync(Guid commentId, string userId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED]->(c:Comment {id: $commentId})
            RETURN COUNT(r) > 0
        ";

        var parameters = new Dictionary<string, object>
        {
            { "commentId", commentId.ToString() },
            { "userId", userId }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return result[0].As<bool>();
        }
        catch
        {
            return false;
        }
    }
}