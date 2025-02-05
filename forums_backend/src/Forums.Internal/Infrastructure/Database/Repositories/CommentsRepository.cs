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

        try {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return new Comment(
                Guid.Parse(result["c.id"].As<string>()),
                result["c.content"].As<string>(),
                result["c.createdAt"].As<string>().FromNeo4jDateTime()
            );
        } catch {
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

        try {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return new Comment(
                Guid.Parse(result["c.id"].As<string>()),
                result["c.content"].As<string>(),
                result["c.createdAt"].As<string>().FromNeo4jDateTime()
            );
        } catch {
            return null;
        }
    }

    public async Task<PostAndComments?> GetPostAndItsCommentsAsync(Guid postId)
    {
        var query = @"MATCH (p:Post {id: $postId})
                    OPTIONAL MATCH (c:Comment)-[:BELONGS_TO]->(p)
                    OPTIONAL MATCH (u:User)-[:COMMENTED]->(c)
                    RETURN p.id, p.title, p.content, p.createdAt, 
                           c.id AS commentId, c.content AS commentContent, c.createdAt AS commentCreatedAt,
                           u.id AS userId, u.username AS username, u.role AS userRole, u.email AS userEmail";

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
            Guid.Parse(firstRecord["p.id"].As<string>()),
            firstRecord["p.title"].As<string>(),
            firstRecord["p.content"].As<string>(),
            firstRecord["p.createdAt"].As<string>().FromNeo4jDateTime()
        );

        var comments = records
            .Where(r => r["commentId"] != null)
            .Select(r => new CommentWithUser(
                new Comment(
                    Guid.Parse(r["commentId"].As<string>()),
                    r["commentContent"].As<string>(),
                    r["commentCreatedAt"].As<string>().FromNeo4jDateTime()
                ),
                new User(
                    r["userId"].As<string>(),
                    r["username"].As<string>(),
                    r["userRole"].As<string>(),
                    r["userEmail"].As<string>()
                )
            )).ToList();

        return new PostAndComments(post, comments);
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