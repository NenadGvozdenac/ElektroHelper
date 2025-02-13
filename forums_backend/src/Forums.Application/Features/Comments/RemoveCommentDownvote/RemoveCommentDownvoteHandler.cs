using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Comments.RemoveCommentDownvote;

public class RemoveCommentDownvoteHandler(IGraphDatabaseContext context) : IRequestHandler<RemoveCommentDownvoteCommand, Result<CommentDownvoteDTO>>
{
    public async Task<Result<CommentDownvoteDTO>> Handle(RemoveCommentDownvoteCommand request, CancellationToken cancellationToken)
    {
        if (!await CommentExists(request.CommentId))
        {
            return Result<CommentDownvoteDTO>.Failure("Comment does not exist").WithCode(404);
        }

        if (!await UserDownvotedComment(request.UserDTO.Id, request.CommentId))
        {
            return Result<CommentDownvoteDTO>.Failure("User has not downvoted comment").WithCode(400);
        }

        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED]->(c:Comment {id: $commentId})
            DELETE r
        ";

        var parameters = new Dictionary<string, object>
        {
            { "userId", request.UserDTO.Id },
            { "commentId", request.CommentId.ToString() }
        };

        try
        {
            await context.RunAsync(query, parameters);
            return Result<CommentDownvoteDTO>.Success(
                new CommentDownvoteDTO(request.CommentId, false)
            );
        }
        catch
        {
            return Result<CommentDownvoteDTO>.Failure("Failed to remove downvote from comment.");
        }
    }

    private async Task<bool> UserDownvotedComment(string id, Guid commentId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED]->(c:Comment {id: $commentId})
            RETURN r";

        var parameters = new Dictionary<string, object>
        {
            { "userId", id },
            { "commentId", commentId.ToString() }
        };

        try
        {
            var result = await context.RunAsync(query, parameters);
            return await result.SingleAsync() != null;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> CommentExists(Guid commentId)
    {
        var query = @"
            MATCH (c:Comment {id: $commentId})
            RETURN c";

        var parameters = new Dictionary<string, object>
        {
            { "commentId", commentId.ToString() }
        };

        try
        {
            var result = await context.RunAsync(query, parameters);
            return await result.SingleAsync() != null;
        }
        catch
        {
            return false;
        }
    }
}
