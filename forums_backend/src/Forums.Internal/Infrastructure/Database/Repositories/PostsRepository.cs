using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
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

    public async Task<Post?> AddAsync(Post post, Guid forumId, User user)
    {
        var query = @"
            MERGE (u:User {id: $userId})
            ON CREATE SET u.email = $email, u.username = $username, u.role = $role
            WITH u
            MATCH (f:Forum)
            WHERE f.id = $forumId
            CREATE (p:Post {id: $id, title: $title, content: $content, createdAt: $createdAt})
            CREATE (f)-[:HAS_POST]->(p)
            CREATE (u)-[:POSTED]->(p)
            RETURN p";

        var parameters = new Dictionary<string, object>
        {
            { "forumId", forumId.ToString() },
            { "userId", user.Id },
            { "email", user.Email },
            { "username", user.Username },
            { "role", user.Role },
            { "id", post.Id.ToString() },
            { "title", post.Title },
            { "content", post.Content },
            { "createdAt", post.CreatedAt.ToNeo4jDateTime() }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            var postNode = result["p"].As<INode>();

            return new Post(
                Guid.Parse(postNode["id"].As<string>()),
                postNode["title"].As<string>(),
                postNode["content"].As<string>(),
                postNode["createdAt"].As<string>().FromNeo4jDateTime()
            );
        }
        catch
        {
            return null;
        }
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
                postNode["content"].As<string>(),
                postNode["createdAt"].As<string>().FromNeo4jDateTime()
            );
        });

        return posts;
    }

    public async Task<Post?> GetByIdAsync(Guid postId)
    {
        var query = @"
            MATCH (p:Post)
            WHERE p.id = $postId
            RETURN p";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() }
        };

        try {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            var postNode = result["p"].As<INode>();

            return new Post(
                Guid.Parse(postNode["id"].As<string>()),
                postNode["title"].As<string>(),
                postNode["content"].As<string>(),
                postNode["createdAt"].As<string>().FromNeo4jDateTime()
            );
        } catch {
            return null;
        }
    }

    public async Task<IEnumerable<ForumAndPosts>> GetMyForumsAndPostsAsync(User user)
    {
        var query = @"
            MATCH (u:User)-[:CREATED]->(f:Forum)-[:HAS_POST]->(p:Post)
            WHERE u.id = $userId
            RETURN f, collect(p) AS posts";

        var parameters = new Dictionary<string, object>
        {
            { "userId", user.Id }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.ToListAsync();

        var forumsAndPosts = result.Select(record =>
        {
            var forumNode = record["f"].As<INode>();
            var postsNodes = record["posts"].As<IEnumerable<INode>>();

            var forum = new Forum(
                Guid.Parse(forumNode["id"].As<string>()),
                forumNode["name"].As<string>(),
                forumNode["description"].As<string>(),
                forumNode["createdAt"].As<string>().FromNeo4jDateTime()
            );

            var posts = postsNodes.Select(postNode =>
            {
                return new Post(
                    Guid.Parse(postNode["id"].As<string>()),
                    postNode["title"].As<string>(),
                    postNode["content"].As<string>(),
                    postNode["createdAt"].As<string>().FromNeo4jDateTime()
                );
            }).ToList();

            return new ForumAndPosts(forum, posts);
        });

        return forumsAndPosts;
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
                postNode["content"].As<string>(),
                postNode["createdAt"].As<string>().FromNeo4jDateTime()
            );
        });

        return posts;
    }
}