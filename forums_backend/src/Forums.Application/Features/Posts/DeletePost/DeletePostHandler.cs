using forums_backend.src.Forums.Application.BackgroundServices;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.DeletePost;

public class DeletePostHandler(IGraphDatabaseContext context, PostSyncService postSyncService) : IRequestHandler<DeletePostCommand, Result<DeletePostDTO>>
{
    public async Task<Result<DeletePostDTO>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        if (!await PostExists(request.PostId))
        {
            return Result<DeletePostDTO>.Failure("Post not found").WithCode(404);
        }

        if (!await UserPostedPost(request.UserDTO.Id, request.PostId))
        {
            return Result<DeletePostDTO>.Failure("User is not the author of the post").WithCode(403);
        }

        if(await IsPostDeleted(request.PostId))
        {
            return Result<DeletePostDTO>.Failure("Post is already deleted").WithCode(400);
        }

        var query = @"
            MATCH (p:Post {id: $postId})
            SET p.isDeleted = true
            RETURN p";

        var parameters = new Dictionary<string, object> {
            { "postId", request.PostId.ToString() }
        };

        await context.RunAsync(query, parameters);

        await postSyncService.ManuallySyncPostsAsync(cancellationToken);

        return Result<DeletePostDTO>.Success(new DeletePostDTO(request.PostId, true));
    }

    private async Task<bool> IsPostDeleted(Guid postId)
    {
        var query = @"
            MATCH (p:Post {id: $postId})
            WHERE p.isDeleted = true
            RETURN p";
        
        var parameters = new Dictionary<string, object> {
            { "postId", postId.ToString() }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }

    private async Task<bool> UserPostedPost(string id, Guid postId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[:POSTED]->(p:Post {id: $postId})
            RETURN p";

        var parameters = new Dictionary<string, object> {
            { "userId", id },
            { "postId", postId.ToString() }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }

    private async Task<bool> PostExists(Guid postId)
    {
        var query = @"
            MATCH (p:Post {id: $postId})
            RETURN p";

        var parameters = new Dictionary<string, object> {
            { "postId", postId.ToString() }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }
}
