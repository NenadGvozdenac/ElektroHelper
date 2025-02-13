using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.RemovePostDownvote;

public class RemovePostDownvoteHandler(IGraphDatabaseContext context) : IRequestHandler<RemovePostDownvoteCommand, Result<PostDownvoteDTO>>
{
    public async Task<Result<PostDownvoteDTO>> Handle(RemovePostDownvoteCommand request, CancellationToken cancellationToken)
    {
        if (!await PostExists(request.PostId))
        {
            return Result<PostDownvoteDTO>.Failure("Post does not exist.").WithCode(404);
        }

        if (!await UserDownvotedPost(request.UserDTO.Id, request.PostId))
        {
            return Result<PostDownvoteDTO>.Failure("User has not downvoted post.").WithCode(400);
        }

        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED_POST]->(p:Post {id: $postId})
            DELETE r";

        var parameters = new Dictionary<string, object> {
            { "userId", request.UserDTO.Id },
            { "postId", request.PostId.ToString() }
        };

        try
        {
            await context.RunAsync(query, parameters);

            return Result<PostDownvoteDTO>.Success(new PostDownvoteDTO(request.PostId, false));
        }
        catch (Exception e)
        {
            return Result<PostDownvoteDTO>.Failure(e.Message);
        }
    }

    private async Task<bool> UserDownvotedPost(string id, Guid postId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED_POST]->(p:Post {id: $postId})
            RETURN r";

        var parameters = new Dictionary<string, object> {
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

        var parameters = new Dictionary<string, object> {
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
