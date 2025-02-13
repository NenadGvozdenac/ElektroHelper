using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Comments.RemoveCommentUpvote;

public class RemoveCommentUpvoteHandler(IGraphDatabaseContext context) : IRequestHandler<RemoveCommentUpvoteCommand, Result<CommentUpvoteDTO>>
{
    public async Task<Result<CommentUpvoteDTO>> Handle(RemoveCommentUpvoteCommand request, CancellationToken cancellationToken)
    {
        if (!await CommentExists(request.CommentId))
        {
            return Result<CommentUpvoteDTO>.Failure("Comment does not exist").WithCode(404);
        }

        if (!await UserUpvotedComment(request.UserDTO.Id, request.CommentId))
        {
            return Result<CommentUpvoteDTO>.Failure("User has not upvoted comment").WithCode(400);
        }

        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED]->(c:Comment {id: $commentId})
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
            return Result<CommentUpvoteDTO>.Success(
                new CommentUpvoteDTO(request.CommentId, false)
            );
        }
        catch
        {
            return Result<CommentUpvoteDTO>.Failure("Failed to remove upvote from comment");
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

    private async Task<bool> UserUpvotedComment(string userId, Guid commentId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED]->(c:Comment {id: $commentId})
            RETURN r";

        var parameters = new Dictionary<string, object>
        {
            { "userId", userId },
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
