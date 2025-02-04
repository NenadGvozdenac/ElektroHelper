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

    public async Task<Comment> AddAsync(Comment comment, Guid postId, User user)
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

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.SingleAsync();

        return new Comment(
            Guid.Parse(result["c.id"].As<string>()),
            result["c.content"].As<string>(),
            result["c.createdAt"].As<string>().FromNeo4jDateTime()
        );
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

    public async Task<Comment> GetByIdAsync(Guid commentId)
    {
        var query = @"MATCH (c:Comment {id: $commentId}) RETURN c.id, c.content, c.createdAt";

        var parameters = new Dictionary<string, object> {
            { "commentId", commentId.ToString() }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.SingleAsync();

        return new Comment(
            Guid.Parse(result["c.id"].As<string>()),
            result["c.content"].As<string>(),
            result["c.createdAt"].As<string>().FromNeo4jDateTime()
        );
    }

    public async Task<PostAndComments> GetPostAndItsCommentsAsync(Guid postId)
    {
        var query = @"
            MATCH (p:Post {id: $postId})<-[:BELONGS_TO]-(c:Comment)
            WHERE p.id = $postId
            RETURN p, collect(c) AS comments";

        var parameters = new Dictionary<string, object> {
            { "postId", postId.ToString() }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.SingleAsync();

        var postNode = result["p"].As<INode>();
        var commentsNodes = result["comments"].As<IEnumerable<INode>>();

        var post = new Post(
            Guid.Parse(postNode["id"].As<string>()),
            postNode["title"].As<string>(),
            postNode["content"].As<string>(),
            postNode["createdAt"].As<string>().FromNeo4jDateTime()
        );

        var comments = commentsNodes.Select(commentNode => new Comment(
            Guid.Parse(commentNode["id"].As<string>()),
            commentNode["content"].As<string>(),
            commentNode["createdAt"].As<string>().FromNeo4jDateTime()
        ));

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