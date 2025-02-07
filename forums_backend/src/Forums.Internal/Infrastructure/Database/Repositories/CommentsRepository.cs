using System.Collections;
using AutoMapper;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly IGraphDatabaseContext _graphDatabaseContext;
    private readonly IMapper _mapper;

    public CommentsRepository(IGraphDatabaseContext graphDatabaseContext, IMapper mapper)
    {
        _graphDatabaseContext = graphDatabaseContext;
        _mapper = mapper;
    }

    public async Task<Comment?> AddAsync(Comment comment, Guid postId, User user)
    {
        var query = @"MATCH (p:Post {id: $postId}), (u:User {id: $userId})
            CREATE (c:Comment {id: $id, content: $content, createdAt: $createdAt, isDeleted: false})
            CREATE (u)-[:COMMENTED]->(c)-[:BELONGS_TO]->(p)
            RETURN c";

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

            return _mapper.Map<Comment>(result["c"].As<INode>());
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        var query = @"MATCH (c:Comment) RETURN c";

        var resultCursor = await _graphDatabaseContext.RunAsync(query);

        return await resultCursor.ToListAsync(record => _mapper.Map<Comment>(record["c"].As<INode>()));
    }

    public async Task<Comment?> GetByIdAsync(Guid commentId)
    {
        var query = @"MATCH (c:Comment {id: $commentId}) RETURN c";

        var parameters = new Dictionary<string, object> {
            { "commentId", commentId.ToString() }
        };

        try
        {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return _mapper.Map<Comment>(result["c"].As<INode>());
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
            OPTIONAL MATCH (p)<-[upvotedPost:UPVOTED_POST]-(postUpvoter:User)
            OPTIONAL MATCH (p)<-[downvotedPost:DOWNVOTED_POST]-(postDownvoter:User)
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
                }) AS downvotes,
                COLLECT(DISTINCT {
                    id: postUpvoter.id,
                    username: postUpvoter.username,
                    role: postUpvoter.role,
                    email: postUpvoter.email,
                    createdAt: upvotedPost.createdAt
                }) AS postUpvotes,
                COLLECT(DISTINCT {
                    id: postDownvoter.id,
                    username: postDownvoter.username,
                    role: postDownvoter.role,
                    email: postDownvoter.email,
                    createdAt: downvotedPost.createdAt
                }) AS postDownvotes
            RETURN 
                originalPoster.id AS originalPosterId, originalPoster.username AS originalPosterUsername, originalPoster.role AS originalPosterRole, originalPoster.email AS originalPosterEmail, 
                p.id AS postId, p.title AS postTitle, p.content AS postContent, p.createdAt AS postCreatedAt, 
                c.id AS commentId, c.content AS commentContent, c.createdAt AS commentCreatedAt, c.isDeleted AS commentIsDeleted, 
                u.id AS commenterId, u.username AS commenterUsername, u.role AS commenterRole, u.email AS commenterEmail, 
                upvotes, downvotes, postUpvotes, postDownvotes;
            ";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
        var records = await resultCursor.ToListAsync();

        if (!records.Any())
            return null;

        var firstRecord = records.First();

        var originalPoster = new User(
            firstRecord["originalPosterId"].As<string>(),
            firstRecord["originalPosterUsername"].As<string>(),
            firstRecord["originalPosterRole"].As<string>(),
            firstRecord["originalPosterEmail"].As<string>()
        );

        var comments = records
            .Where(r => r["commentId"] != null)
            .Select(r => new CommentWithUpvotesAndDownvotes(
                new Comment{
                    Id = Guid.Parse(r["commentId"].As<string>()),
                    Content = r["commentContent"].As<string>(),
                    CreatedAt = r["commentCreatedAt"].As<string>().FromNeo4jDateTime(),
                    IsDeleted = r["commentIsDeleted"].As<bool>(),
                    NumberOfUpvotes = r["upvotesCount"].As<int>(),
                    NumberOfDownvotes = r["downvotesCount"].As<int>()
                },
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

        var postUpvotes = firstRecord["postUpvotes"].As<List<Dictionary<string, object>>>()
            .Where(upvote => upvote["id"] != null)
            .Select(upvote => new UserUpvote(
                new User(
                    upvote["id"].As<string>(),
                    upvote["username"].As<string>(),
                    upvote["role"].As<string>(),
                    upvote["email"].As<string>()
                ),
                upvote["createdAt"].As<string>().FromNeo4jDateTime()
            )).ToList() ?? new List<UserUpvote>();

        var postDownvotes = firstRecord["postDownvotes"].As<List<Dictionary<string, object>>>()
            .Where(downvote => downvote["id"] != null)
            .Select(downvote => new UserDownvote(
                new User(
                    downvote["id"].As<string>(),
                    downvote["username"].As<string>(),
                    downvote["role"].As<string>(),
                    downvote["email"].As<string>()
                ),
                downvote["createdAt"].As<string>().FromNeo4jDateTime()
            )).ToList() ?? new List<UserDownvote>();

        var post = new Post{
            Id = Guid.Parse(firstRecord["postId"].As<string>()),
            Title = firstRecord["postTitle"].As<string>(),
            Content = firstRecord["postContent"].As<string>(),
            CreatedAt = firstRecord["postCreatedAt"].As<string>().FromNeo4jDateTime(),
            NumberOfUpvotes = postUpvotes.Count,
            NumberOfDownvotes = postDownvotes.Count
        };

        return new PostAndCommentsWithUpvotesAndDownvotes(post, originalPoster, comments, postUpvotes, postDownvotes);
    }

    public async Task<IEnumerable<Comment>> GetMyCommentsAsync(User user)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[:COMMENTED]->(c:Comment)
            RETURN c";

        var parameters = new Dictionary<string, object> {
            { "userId", user.Id.ToString() }
        };

        var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);

        return await resultCursor.ToListAsync(record => _mapper.Map<Comment>(record["c"].As<INode>()));
    }
}