using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Comments.DownvoteComment;

public class DownvoteCommentHandler(IGraphDatabaseContext context) : IRequestHandler<DownvoteCommentCommand, Result<DownvoteCommentDTO>>
{
    public async Task<Result<DownvoteCommentDTO>> Handle(DownvoteCommentCommand request, CancellationToken cancellationToken)
    {
        if (!await CommentExists(request.CommentId))
        {
            return Result<DownvoteCommentDTO>.Failure("Comment does not exist.").WithCode(404);
        }

        if (await UserHasDownvotedComment(request.CommentId, request.UserDTO.Id))
        {
            return Result<DownvoteCommentDTO>.Failure("User has already downvoted this comment.").WithCode(400);
        }

        if (await UserHasUpvotedComment(request.CommentId, request.UserDTO.Id))
        {
            await RemoveUpvote(request.CommentId, request.UserDTO.Id);
        }

        var query = @"
            MATCH (c:Comment {id: $commentId})
            MATCH (u:User {id: $userId})
            MERGE (u)-[r:DOWNVOTED]->(c)
            ON CREATE SET r.downvotedAt = $downvotedAt
            RETURN COUNT(r) > 0
        ";

        var parameters = new Dictionary<string, object>
        {
            { "commentId", request.CommentId.ToString() },
            { "userId", request.UserDTO.Id },
            { "downvotedAt", DateTime.UtcNow.ToNeo4jDateTime() }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            return Result<DownvoteCommentDTO>.Success(new DownvoteCommentDTO(
                request.CommentId,
                true
            ));
        }
        catch
        {
            return Result<DownvoteCommentDTO>.Failure("Failed to downvote comment.");
        }
    }

    private async Task RemoveUpvote(Guid commentId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED]->(c:Comment {id: $commentId})
            DELETE r";

        var parameters = new Dictionary<string, object>
        {
            { "userId", id },
            { "commentId", commentId.ToString() }
        };

        try
        {
            await context.RunAsync(query, parameters);
        }
        catch
        {
            return;
        }
    }

    private async Task<bool> UserHasUpvotedComment(Guid commentId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:UPVOTED]->(c:Comment {id: $commentId})
            RETURN r";

        var parameters = new Dictionary<string, object>
        {
            { "userId", id },
            { "commentId", commentId.ToString() }
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

    private async Task<bool> UserHasDownvotedComment(Guid commentId, string id)
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
            var resultCursor = await context.RunAsync(query, parameters);
            return await resultCursor.FetchAsync();
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
            var resultCursor = await context.RunAsync(query, parameters);
            return await resultCursor.FetchAsync();
        }
        catch
        {
            return false;
        }
    }
}
