using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.DownvotePost;

public class DownvotePostHandler(IGraphDatabaseContext context) : IRequestHandler<DownvotePostCommand, Result<DownvotePostDTO>>
{
    public async Task<Result<DownvotePostDTO>> Handle(DownvotePostCommand request, CancellationToken cancellationToken)
    {
        if(!await PostExists(request.PostId)) {
            return Result<DownvotePostDTO>.Failure("Post does not exist").WithCode(404);
        }

        if(await UserDownvotedPost(request.PostId, request.UserDTO.Id)) {
            return Result<DownvotePostDTO>.Failure("User already downvoted post").WithCode(400);
        }

        if(await UserUpvotedPost(request.PostId, request.UserDTO.Id)) {
            await RemoveUpvote(request.PostId, request.UserDTO.Id);
        }

        var query = @"
            MATCH (p:Post {id: $postId})
            MATCH (u:User {id: $userId})
            MERGE (u)-[r:DOWNVOTED_POST]->(p)
            ON CREATE SET r.downvotedAt = $downvotedAt
            RETURN COUNT(r) > 0";

        var parameters = new Dictionary<string, object>
        {
            { "postId", request.PostId.ToString() },
            { "userId", request.UserDTO.Id },
            { "downvotedAt", DateTime.UtcNow.ToNeo4jDateTime() }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            return Result<DownvotePostDTO>.Success(new DownvotePostDTO(
                request.PostId,
                true
            ));
        }
        catch
        {
            return Result<DownvotePostDTO>.Failure("Failed to downvote post");
        }
    }

    private async Task RemoveUpvote(Guid postId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED_POST]->(p:Post {id: $postId})
            DELETE r";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() },
            { "userId", id }
        };

        await context.RunAsync(query, parameters);
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

    private async Task<bool> PostExists(Guid postId)
    {
        var query = @"
            MATCH (p:Post {id: $postId})
            WHERE p.isDeleted = false
            RETURN p";

        var parameters = new Dictionary<string, object>
        {
            { "postId", postId.ToString() }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }
}
