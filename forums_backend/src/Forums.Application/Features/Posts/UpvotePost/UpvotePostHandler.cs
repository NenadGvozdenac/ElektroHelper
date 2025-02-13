using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.UpvotePost;

public class UpvotePostHandler(IGraphDatabaseContext context) : IRequestHandler<UpvotePostCommand, Result<UpvotePostDTO>>
{
    public async Task<Result<UpvotePostDTO>> Handle(UpvotePostCommand request, CancellationToken cancellationToken)
    {
        if (!await PostExists(request.PostId))
        {
            return Result<UpvotePostDTO>.Failure("Post does not exist").WithCode(404);
        }

        if(await UserUpvotedPost(request.PostId, request.UserDTO.Id))
        {
            return Result<UpvotePostDTO>.Failure("User already upvoted post").WithCode(400);
        }

        if(await UserDownvotedPost(request.PostId, request.UserDTO.Id))
        {
            await RemoveDownvote(request.PostId, request.UserDTO.Id);
        }

        var query = @"
            MATCH (p:Post {id: $postId})
            MATCH (u:User {id: $userId})
            MERGE (u)-[r:UPVOTED_POST]->(p)
            ON CREATE SET r.upvotedAt = $upvotedAt
            RETURN COUNT(r) > 0";

        var parameters = new Dictionary<string, object>
        {
            { "postId", request.PostId.ToString() },
            { "userId", request.UserDTO.Id },
            { "upvotedAt", DateTime.UtcNow.ToNeo4jDateTime() }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            return Result<UpvotePostDTO>.Success(new UpvotePostDTO(
                request.PostId,
                true
            ));
        }
        catch
        {
            return Result<UpvotePostDTO>.Failure("Failed to upvote post");
        }
    }

    private async Task RemoveDownvote(Guid postId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED_POST]->(p:Post {id: $postId})
            DELETE r";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", id }
        };

        await context.RunAsync(query, parameters);
    }

    private async Task<bool> UserDownvotedPost(Guid postId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED_POST]->(p:Post {id: $postId})
            RETURN r";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", id }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }

    private async Task<bool> UserUpvotedPost(Guid postId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED_POST]->(p:Post {id: $postId})
            RETURN r";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", id }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }

    private async Task<bool> PostExists(Guid postId)
    {
        var query = @"
            MATCH (p:Post {id: $postId})
            RETURN p";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }
}
