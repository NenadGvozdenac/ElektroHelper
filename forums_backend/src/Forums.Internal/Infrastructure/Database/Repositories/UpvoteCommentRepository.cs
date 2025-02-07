using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class UpvoteCommentRepository : IUpvoteCommentRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;

    public UpvoteCommentRepository(IGraphDatabaseContext graphDatabaseContext)
    {
        _graphDatabaseContext = graphDatabaseContext;
    }

    public async Task<bool> AddUpvoteToCommentAsync(Guid commentId, string userId)
    {
        var upvotedAt = DateTime.UtcNow;

        var query = @"
            MATCH (c:Comment {id: $commentId})
            MATCH (u:User {id: $userId})
            MERGE (u)-[r:UPVOTED]->(c)
            ON CREATE SET r.upvotedAt = $upvotedAt
            RETURN COUNT(r) > 0
        ";

        var parameters = new Dictionary<string, object>
        {
            { "commentId", commentId.ToString() },
            { "userId", userId },
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

    public async Task<CommentWithUpvotes?> GetCommentWithUpvotesAsync(Guid commentId)
    {
        var query = @"
            MATCH (c:Comment {id: $commentId})
            WITH c
            OPTIONAL MATCH (c)<-[r:UPVOTED]-(user:User)
            OPTIONAL MATCH (c)<-[r2:DOWNVOTES]-(user2:User)
            RETURN c, COLLECT({ user: user, upvoteDate: r.upvotedAt }) AS userUpvotes, count(r) AS upvotes, count(r2) AS downvotes
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
            var userUpvotesNodes = result["userUpvotes"].As<List<Dictionary<string, object>>>();

            var comment = new Comment{
                Id = Guid.Parse(commentNode["id"].As<string>()),
                Content = commentNode["content"].As<string>(),
                CreatedAt = commentNode["createdAt"].As<string>().FromNeo4jDateTime(),
                NumberOfUpvotes = result["upvotes"].As<int>(),
                NumberOfDownvotes = result["downvotes"].As<int>()
            };

            var upvotes = userUpvotesNodes.Select(userUpvoteNode =>
            {
                var userNode = userUpvoteNode["user"].As<INode>();
                var upvoteDate = userUpvoteNode["upvoteDate"].As<string>().FromNeo4jDateTime();

                var user = new User(
                    userNode["id"].As<string>(),
                    userNode["email"].As<string>(),
                    userNode["role"].As<string>(),
                    userNode["username"].As<string>()
                );

                var userUpvote = new UserUpvote(user, upvoteDate);

                return userUpvote;
            });

            return new CommentWithUpvotes(comment, upvotes);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> RemoveUpvoteFromCommentAsync(Guid commentId, string userId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED]->(c:Comment {id: $commentId})
            DELETE r
        ";

        var parameters = new Dictionary<string, object>
        {
            { "commentId", commentId.ToString() },
            { "userId", userId }
        };

        try
        {
            await _graphDatabaseContext.RunAsync(query, parameters);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveUpvoteFromCommentIfExistsAsync(Guid commentId, string userId)
    {
        var userUpvotedComment = await UserUpvotedCommentAsync(commentId, userId);

        if (userUpvotedComment)
        {
            var removedUpvote = await RemoveUpvoteFromCommentAsync(commentId, userId);

            return removedUpvote;
        }

        return true;
    }

    public async Task<bool> UserUpvotedCommentAsync(Guid commentId, string userId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED]->(c:Comment {id: $commentId})
            RETURN COUNT(r) > 0 AS userUpvoted
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

            return bool.Parse(result["userUpvoted"].As<string>());
        }
        catch
        {
            return false;
        }
    }
}