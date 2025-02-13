using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.RemovePostUpvote;

public class RemovePostUpvoteHandler(IGraphDatabaseContext context) : IRequestHandler<RemovePostUpvoteCommand, Result<UpvotePostDTO>>
{
    public async Task<Result<UpvotePostDTO>> Handle(RemovePostUpvoteCommand request, CancellationToken cancellationToken)
    {
        if (!await PostExists(request.PostId))
        {
            return Result<UpvotePostDTO>.Failure("Post does not exist.").WithCode(404);
        }

        if (!await UserUpvotedPost(request.UserDTO.Id, request.PostId))
        {
            return Result<UpvotePostDTO>.Failure("User has not upvoted post.").WithCode(400);
        }

        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED_POST]->(p:Post {id: $postId})
            DELETE r";

        var parameters = new Dictionary<string, object>
        {
            { "postId", request.PostId.ToString() },
            { "userId", request.UserDTO.Id }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            return Result<UpvotePostDTO>.Success(
                new UpvotePostDTO(request.PostId, false)
            );
        }
        catch
        {
            return Result<UpvotePostDTO>.Failure("Failed to remove upvote from post.");
        }
    }

    private async Task<bool> UserUpvotedPost(string id, Guid postId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED_POST]->(p:Post {id: $postId})
            RETURN r";

        var parameters = new Dictionary<string, object>
        {
            { "userId", id },
            { "postId", postId.ToString() }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            return await resultCursor.FetchAsync();
        }
        catch
        {
            return false;
        }
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

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            return await resultCursor.FetchAsync();
        }
        catch
        {
            return false;
        }
    }
}
