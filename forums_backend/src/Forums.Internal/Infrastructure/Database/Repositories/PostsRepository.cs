using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class PostsRepository : IPostsRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;

    public PostsRepository(IGraphDatabaseContext graphDatabaseContext)
    {
        _graphDatabaseContext = graphDatabaseContext;
    }

    public async Task<Post> AddAsync(Post post, Guid forumId)
    {
        var query = @"
            MATCH (f:Forum)
            WHERE f.id = $forumId
            CREATE (p:Post {id: $id, title: $title, content: $content})
            CREATE (f)-[:HAS_POST]->(p)
            RETURN p";

        var parameters = new Dictionary<string, object>
        {
            { "forumId", forumId.ToString() },
            { "id", post.Id.ToString() },
            { "title", post.Title },
            { "content", post.Content },
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.SingleAsync();

        var postNode = result["p"].As<INode>();

        return new Post(
            Guid.Parse(postNode["id"].As<string>()),
            postNode["title"].As<string>(),
            postNode["content"].As<string>()
        );
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        var query = "MATCH (p:Post) RETURN p";

        var resultCursor = await _graphDatabaseContext.RunAsync(query);
        var result = await resultCursor.ToListAsync();

        var posts = result.Select(record =>
        {
            var postNode = record["p"].As<INode>();
            return new Post(
                Guid.Parse(postNode["id"].As<string>()),
                postNode["title"].As<string>(),
                postNode["content"].As<string>()
            );
        });

        return posts;
    }

    public async Task<IEnumerable<Post>> GetPostsByForumIdAsync(Guid forumId)
    {
        var query = @"
                MATCH (f:Forum)-[:HAS_POST]->(p:Post)
                WHERE f.id = $forumId
                RETURN p";

        var parameters = new Dictionary<string, object>
        {
            { "forumId", forumId.ToString() }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.ToListAsync();

        var posts = result.Select(record =>
        {
            var postNode = record["p"].As<INode>();
            return new Post(
                Guid.Parse(postNode["id"].As<string>()),
                postNode["title"].As<string>(),
                postNode["content"].As<string>()
            );
        });

        return posts;
    }
}