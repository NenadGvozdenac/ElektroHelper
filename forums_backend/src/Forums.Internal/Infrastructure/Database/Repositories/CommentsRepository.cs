using System.Collections;
using AutoMapper;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.API.DTOs.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
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

    public async Task<IEnumerable<CommentAndVoting>> GetCommentsForPost(Guid postId, UserDTO userDTO)
    {
        var query = @"
            MATCH (p:Post {id: $postId})<-[:BELONGS_TO]-(c:Comment)
            OPTIONAL MATCH (u1:User {id: $userId})-[uv:UPVOTED]->(c)
            OPTIONAL MATCH (u2:User {id: $userId})-[dv:DOWNVOTED]->(c)
            OPTIONAL MATCH (u3)-[uv1:UPVOTED]->(c)
            OPTIONAL MATCH (u4)-[dv1:DOWNVOTED]->(c)
            OPTIONAL MATCH (u5: User)-[:COMMENTED]->(c)
            RETURN c, COUNT(distinct uv1) as upvotes, COUNT(distinct dv1) as downvotes, u5 as author,
                CASE WHEN COUNT(distinct uv) > 0 THEN true ELSE false END as upvoted,
                CASE WHEN COUNT(distinct dv) > 0 THEN true ELSE false END as downvoted";
        
        var parameters = new Dictionary<string, object> {
            { "postId", postId.ToString() },
            { "userId", userDTO.Id.ToString() }
        };

        try {
            var resultCursor = await _graphDatabaseContext.RunAsync(query, parameters);
            var list = await resultCursor.ToListAsync();

            var comments = list.Select(record => {
                var comment = _mapper.Map<Comment>(record["c"].As<INode>());

                comment.NumberOfUpvotes = record["upvotes"].As<int>();
                comment.NumberOfDownvotes = record["downvotes"].As<int>();

                var author = record["author"].As<INode>();
                var upvoted = record["upvoted"].As<bool>();
                var downvoted = record["downvoted"].As<bool>();

                return new CommentAndVoting {
                    Comment = comment,
                    Author = _mapper.Map<User>(author),
                    IsUpvoted = upvoted,
                    IsDownvoted = downvoted,
                };
            });

            return comments;
        } catch {
            return new List<CommentAndVoting>();
        }
    }
}