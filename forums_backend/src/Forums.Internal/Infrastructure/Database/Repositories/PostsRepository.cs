using AutoMapper;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class PostsRepository : IPostsRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;
    private readonly IMapper _mapper;

    public PostsRepository(IGraphDatabaseContext graphDatabaseContext, IMapper mapper)
    {
        _graphDatabaseContext = graphDatabaseContext;
        _mapper = mapper;
    }

    public async Task<Post?> AddAsync(Post post, Guid forumId, User user)
    {
        var query = @"
            MERGE (u:User {id: $userId})
            ON CREATE SET u.email = $email, u.username = $username, u.role = $role
            WITH u
            MATCH (f:Forum)
            WHERE f.id = $forumId
            CREATE (p:Post {id: $id, title: $title, content: $content, createdAt: $createdAt, isDeleted: false, isLocked: false})
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
            return _mapper.Map<Post>(result["p"].As<INode>());
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        var query = @"
            MATCH (p:Post)
            OPTIONAL MATCH (u1:User)-[upvote:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u2:User)-[downvote:DOWNVOTED_POST]->(p)
            RETURN p, count(upvote) AS upvotes, count(downvote) AS downvotes";

        var resultCursor = await _graphDatabaseContext.RunAsync(query);
        var result = await resultCursor.ToListAsync();

        return result.Select(record =>
        {
            var post = _mapper.Map<Post>(record["p"].As<INode>());
            post.SetNumberOfUpvotes(record["upvotes"].As<int>());
            post.SetNumberOfDownvotes(record["downvotes"].As<int>());
            return post;
        });
    }

    public async Task<IEnumerable<PostVoting>> GetPagedAsync(int page, int pageSize, UserDTO userDTO)
    {
        var query = @"
            MATCH (p:Post)
            OPTIONAL MATCH (u1:User)-[upvote1:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u2:User)-[downvote1:DOWNVOTED_POST]->(p)
            OPTIONAL MATCH (c1:Comment)-[comment:BELONGS_TO]->(p)
            OPTIONAL MATCH (u4:User {id: $userId})-[upvote2:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u5:User {id: $userId})-[downvote2:DOWNVOTED_POST]->(p)
            OPTIONAL MATCH (u6:User)-[:POSTED]->(p)
            RETURN p, 
                count(upvote1) AS upvotes, 
                count(downvote1) AS downvotes, 
                count(comment) AS comments,
                u6 AS author,
                CASE WHEN u4 IS NOT NULL THEN true ELSE false END AS hasUpvoted,
                CASE WHEN u5 IS NOT NULL THEN true ELSE false END AS hasDownvoted
            SKIP $skip
            LIMIT $limit";

        var parameters = new Dictionary<string, object>
        {
            { "skip", (page - 1) * pageSize },
            { "limit", pageSize },
            { "userId", userDTO.Id }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.ToListAsync();

        var posts = result.Select(record =>
        {
            var post = _mapper.Map<Post>(record["p"].As<INode>());
            post.SetNumberOfUpvotes(record["upvotes"].As<int>());
            post.SetNumberOfDownvotes(record["downvotes"].As<int>());
            post.SetNumberOfComments(record["comments"].As<int>());

            var hasUpvoted = bool.Parse(record["hasUpvoted"].As<string>());
            var hasDownvoted = bool.Parse(record["hasDownvoted"].As<string>());
            var author = _mapper.Map<User>(record["author"].As<INode>());

            return new PostVoting
            {
                Post = post,
                IsUpvoted = hasUpvoted,
                IsDownvoted = hasDownvoted,
                Author = author
            };
        });

        return posts;
    }

    public async Task<IEnumerable<PostVoting>> GetPostsByForumIdAsync(Guid forumId, UserDTO userDTO)
    {
        var query = @"
                MATCH (f:Forum)-[:HAS_POST]->(p:Post)
                WHERE f.id = $forumId
                OPTIONAL MATCH (u1:User)-[upvote:UPVOTED_POST]->(p)
                OPTIONAL MATCH (u2:User)-[downvote:DOWNVOTED_POST]->(p)
                OPTIONAL MATCH (c:Comment)-[comment:BELONGS_TO]->(p)
                OPTIONAL MATCH (u4:User {id: $userId})-[:UPVOTED_POST]->(p)
                OPTIONAL MATCH (u5:User {id: $userId})-[:DOWNVOTED_POST]->(p)
                OPTIONAL MATCH (u6:User)-[:POSTED]->(p)
                RETURN p, count(upvote) AS upvotes, count(downvote) AS downvotes, count(comment) AS comments,
                    u6 AS author, 
                    CASE WHEN u4 IS NOT NULL THEN true ELSE false END AS hasUpvoted,
                    CASE WHEN u5 IS NOT NULL THEN true ELSE false END AS hasDownvoted";

        var parameters = new Dictionary<string, object> {
            { "forumId", forumId.ToString() } ,
            { "userId", userDTO.Id }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.ToListAsync();

        return result.Select(record =>
        {
            var post = _mapper.Map<Post>(record["p"].As<INode>());
            post.SetNumberOfUpvotes(record["upvotes"].As<int>());
            post.SetNumberOfDownvotes(record["downvotes"].As<int>());
            post.SetNumberOfComments(record["comments"].As<int>());

            var hasUpvoted = bool.Parse(record["hasUpvoted"].As<string>());
            var hasDownvoted = bool.Parse(record["hasDownvoted"].As<string>());
            var author = _mapper.Map<User>(record["author"].As<INode>());

            return new PostVoting
            {
                Post = post,
                IsUpvoted = hasUpvoted,
                IsDownvoted = hasDownvoted,
                Author = author
            };
        });
    }

    public async Task<IEnumerable<PostVoting>> GetPostsByForumIdPagedAsync(int page, int pageSize, Guid forumId, UserDTO userDTO)
    {
        var query = @"
            MATCH (f:Forum)-[:HAS_POST]->(p:Post)
            WHERE f.id = $forumId
            OPTIONAL MATCH (u1:User)-[upvote1:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u2:User)-[downvote1:DOWNVOTED_POST]->(p)
            OPTIONAL MATCH (u3:User {id: $userId})-[upvote2:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u4:User {id: $userId})-[downvote2:DOWNVOTED_POST]->(p)
            OPTIONAL MATCH (u5:User)-[:POSTED]->(p)
            OPTIONAL MATCH (c:Comment)-[comment:BELONGS_TO]->(p)
            RETURN p, 
                count(upvote1) AS upvotes, 
                count(downvote1) AS downvotes, 
                count(comment) AS comments,
                u5 AS author,
                CASE WHEN u3 IS NOT NULL THEN true ELSE false END AS hasUpvoted,
                CASE WHEN u4 IS NOT NULL THEN true ELSE false END AS hasDownvoted
            SKIP $skip
            LIMIT $limit";

        var parameters = new Dictionary<string, object>
        {
            { "forumId", forumId.ToString() },
            { "skip", (page - 1) * pageSize },
            { "limit", pageSize },
            { "userId", userDTO.Id }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.ToListAsync();

        var posts = result.Select(record =>
        {
            var post = _mapper.Map<Post>(record["p"].As<INode>());
            post.SetNumberOfUpvotes(record["upvotes"].As<int>());
            post.SetNumberOfDownvotes(record["downvotes"].As<int>());
            post.SetNumberOfComments(record["comments"].As<int>());

            var hasUpvoted = bool.Parse(record["hasUpvoted"].As<string>());
            var hasDownvoted = bool.Parse(record["hasDownvoted"].As<string>());
            var author = _mapper.Map<User>(record["author"].As<INode>());

            return new PostVoting
            {
                Post = post,
                IsUpvoted = hasUpvoted,
                IsDownvoted = hasDownvoted,
                Author = author
            };
        });

        return posts;
    }

    public async Task<Post?> GetByIdAsync(Guid postId)
    {
        var query = @"
            MATCH (p:Post)
            WHERE p.id = $postId
            OPTIONAL MATCH (u1:User)-[upvote:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u2:User)-[downvote:DOWNVOTED_POST]->(p)
            RETURN p, count(upvote) AS upvotes, count(downvote) AS downvotes";

        var parameters = new Dictionary<string, object> { { "postId", postId.ToString() } };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            var post = _mapper.Map<Post>(result["p"].As<INode>());
            post.SetNumberOfUpvotes(result["upvotes"].As<int>());
            post.SetNumberOfDownvotes(result["downvotes"].As<int>());

            return post;
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<ForumAndPosts>> GetMyForumsAndPostsAsync(User user)
    {
        var query = @"
            MATCH (u:User)-[:CREATED]->(f:Forum)-[:HAS_POST]->(p:Post)
            WHERE u.id = $userId
            OPTIONAL MATCH (upvoter:User)-[:UPVOTED_POST]->(p)
            OPTIONAL MATCH (downvoter:User)-[:DOWNVOTED_POST]->(p)
            OPTIONAL MATCH (f)-[:HAS_POST]->(p1:Post)
            RETURN f, collect({ post: p }) AS posts, count(upvoter) AS upvotes, count(downvoter) AS downvotes, collect(p1) AS numberOfPosts";

        var parameters = new Dictionary<string, object> { { "userId", user.Id } };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var result = await resultCursor.ToListAsync();

        return result.Select(record =>
        {
            var forum = _mapper.Map<Forum>(record["f"].As<INode>());

            forum.NumberOfPosts = record["numberOfPosts"].As<List<INode>>().Count;

            var posts = record["posts"].As<List<INode>>().Select(postNode =>
            {
                var post = _mapper.Map<Post>(postNode["post"].As<INode>());
                post.SetNumberOfUpvotes(record["upvotes"].As<int>());
                post.SetNumberOfDownvotes(record["downvotes"].As<int>());
                return post;
            });

            return new ForumAndPosts(forum, posts);
        });
    }
}