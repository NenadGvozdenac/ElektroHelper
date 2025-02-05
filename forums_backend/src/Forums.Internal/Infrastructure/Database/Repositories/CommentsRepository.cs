using System.Collections;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;

    public CommentsRepository(IGraphDatabaseContext graphDatabaseContext)
    {
        _graphDatabaseContext = graphDatabaseContext;
    }

    public async Task<Comment?> AddAsync(Comment comment, Guid postId, User user)
    {
        var query = @"MATCH (p:Post {id: $postId}), (u:User {id: $userId})
            CREATE (c:Comment {id: $id, content: $content, createdAt: $createdAt})
            CREATE (u)-[:COMMENTED]->(c)-[:BELONGS_TO]->(p)
            RETURN c.id, c.content, c.createdAt";

        var parameters = new Dictionary<string, object> {
            { "id", comment.Id.ToString() },
            { "content", comment.Content },
            { "createdAt", comment.CreatedAt.ToNeo4jDateTime() },
            { "postId", postId.ToString() },
            { "userId", user.Id.ToString() }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return new Comment(
                Guid.Parse(result["c.id"].As<string>()),
                result["c.content"].As<string>(),
                result["c.createdAt"].As<string>().FromNeo4jDateTime()
            );
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        var query = @"MATCH (c:Comment) RETURN c.id, c.content, c.createdAt";

        var resultCursor = await _graphDatabaseContext.RunAsync(query);

        return await resultCursor.ToListAsync(record => new Comment(
            Guid.Parse(record["c.id"].As<string>()),
            record["c.content"].As<string>(),
            record["c.createdAt"].As<string>().FromNeo4jDateTime()
        ));
    }

    public async Task<Comment?> GetByIdAsync(Guid commentId)
    {
        var query = @"MATCH (c:Comment {id: $commentId}) RETURN c.id, c.content, c.createdAt";

        var parameters = new Dictionary<string, object> {
            { "commentId", commentId.ToString() }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return new Comment(
                Guid.Parse(result["c.id"].As<string>()),
                result["c.content"].As<string>(),
                result["c.createdAt"].As<string>().FromNeo4jDateTime()
            );
        }
        catch
        {
            return null;
        }
    }

    public async Task<PostAndCommentsWithUpvotesAndDownvotes?> GetPostAndItsCommentsAsync(Guid postId)
    {
        var query = @"MATCH (p:Post {id: $postId})
        OPTIONAL MATCH (p)<-[:POSTED]-(originalPoster:User)
        OPTIONAL MATCH (c:Comment)-[:BELONGS_TO]->(p)
        OPTIONAL MATCH (u:User)-[:COMMENTED]->(c)
        OPTIONAL MATCH (c)<-[upvote:UPVOTED]-(upvoter:User)
        OPTIONAL MATCH (c)<-[downvote:DOWNVOTED]-(downvoter:User)
        WITH 
            p, originalPoster, c, u, 
            COLLECT(DISTINCT {
                id: upvoter.id, 
                username: upvoter.username, 
                role: upvoter.role, 
                email: upvoter.email, 
                createdAt: upvote.createdAt
            }) AS upvotes,
            COLLECT(DISTINCT {
                id: downvoter.id, 
                username: downvoter.username, 
                role: downvoter.role, 
                email: downvoter.email, 
                createdAt: downvote.createdAt
            }) AS downvotes
        RETURN 
            originalPoster.id AS originalPosterId, originalPoster.username AS originalPosterUsername, originalPoster.role AS originalPosterRole, originalPoster.email AS originalPosterEmail, 
            p.id AS postId, p.title AS postTitle, p.content AS postContent, p.createdAt AS postCreatedAt, 
            c.id AS commentId, c.content AS commentContent, c.createdAt AS commentCreatedAt, 
            u.id AS commenterId, u.username AS commenterUsername, u.role AS commenterRole, u.email AS commenterEmail, 
            upvotes, downvotes";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var records = await resultCursor.ToListAsync();

        if (!records.Any())
            return null;

        var firstRecord = records.First();
        var post = new Post(
            Guid.Parse(firstRecord["postId"].As<string>()),
            firstRecord["postTitle"].As<string>(),
            firstRecord["postContent"].As<string>(),
            firstRecord["postCreatedAt"].As<string>().FromNeo4jDateTime()
        );

        var originalPoster = new User(
            firstRecord["originalPosterId"].As<string>(),
            firstRecord["originalPosterUsername"].As<string>(),
            firstRecord["originalPosterRole"].As<string>(),
            firstRecord["originalPosterEmail"].As<string>()
        );

        var comments = records
            .Where(r => r["commentId"] != null)
            .Select(r => new CommentWithUpvotesAndDownvotes(
                new Comment(
                    Guid.Parse(r["commentId"].As<string>()),
                    r["commentContent"].As<string>(),
                    r["commentCreatedAt"].As<string>().FromNeo4jDateTime()
                ),
                new User(
                    r["commenterId"].As<string>(),
                    r["commenterUsername"].As<string>(),
                    r["commenterRole"].As<string>(),
                    r["commenterEmail"].As<string>()
                ),
                r["upvotes"].As<List<Dictionary<string, object>>>()
                    .Where(upvote => upvote["id"] != null)
                    .Select(upvote => new UserUpvote(
                    new User(
                        upvote["id"].As<string>(),
                        upvote["username"].As<string>(),
                        upvote["role"].As<string>(),
                        upvote["email"].As<string>()
                    ),
                    upvote["createdAt"].As<string>().FromNeo4jDateTime()
                )).ToList() ?? new List<UserUpvote>(),
                r["downvotes"].As<List<Dictionary<string, object>>>()
                    .Where(downvote => downvote["id"] != null)
                    .Select(downvote => new UserDownvote(
                    new User(
                        downvote["id"].As<string>(),
                        downvote["username"].As<string>(),
                        downvote["role"].As<string>(),
                        downvote["email"].As<string>()
                    ),
                    downvote["createdAt"].As<string>().FromNeo4jDateTime()
                )).ToList() ?? new List<UserDownvote>()
            )).ToList();

        return new PostAndCommentsWithUpvotesAndDownvotes(post, originalPoster, comments);
    }

    public async Task<IEnumerable<Comment>> GetMyCommentsAsync(User user)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[:COMMENTED]->(c:Comment)-[:BELONGS_TO]->(p:Post)
            RETURN c.id, c.content, c.createdAt";

        var parameters = new Dictionary<string, object> {
            { "userId", user.Id.ToString() }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);

        return await resultCursor.ToListAsync(record => new Comment(
            Guid.Parse(record["c.id"].As<string>()),
            record["c.content"].As<string>(),
            record["c.createdAt"].As<string>().FromNeo4jDateTime()
        ));
    }
}