using forums_backend.src.Forums.Application.BackgroundServices;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.DeletePostByAdmin;
public class DeletePostByAdminHandler(IGraphDatabaseContext context, PostSyncService postSyncService) : IRequestHandler<DeletePostByAdminCommand, Result<DeletePostDTO>>
{
    public async Task<Result<DeletePostDTO>> Handle(DeletePostByAdminCommand request, CancellationToken cancellationToken)
    {
        if (!await PostExists(request.PostId))
        {
            return Result<DeletePostDTO>.Failure("Post not found").WithCode(404);
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
