using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Comments.UpvoteComment;

public class UpvoteCommentHandler(IGraphDatabaseContext context) : IRequestHandler<UpvoteCommentCommand, Result<UpvoteCommentDTO>>
{
    public async Task<Result<UpvoteCommentDTO>> Handle(UpvoteCommentCommand request, CancellationToken cancellationToken)
    {
        if (!await CommentExists(request.CommentId))
        {
            return Result<UpvoteCommentDTO>.Failure("Comment does not exist.").WithCode(404);
        }

        if (await UserHasUpvotedComment(request.CommentId, request.UserDTO.Id))
        {
            return Result<UpvoteCommentDTO>.Failure("User has already upvoted this comment.").WithCode(400);
        }

        if (await UserHasDownvotedComment(request.CommentId, request.UserDTO.Id))
        {
            await RemoveDownvote(request.CommentId, request.UserDTO.Id);
        }

        var query = @"
            MATCH (c:Comment {id: $commentId})
            MATCH (u:User {id: $userId})
            MERGE (u)-[r:UPVOTED]->(c)
            ON CREATE SET r.upvotedAt = $upvotedAt
            RETURN COUNT(r) > 0";

        var parameters = new Dictionary<string, object>
        {
            { "commentId", request.CommentId.ToString() },
            { "userId", request.UserDTO.Id },
            { "upvotedAt", DateTime.UtcNow.ToNeo4jDateTime() }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            return Result<UpvoteCommentDTO>.Success(new UpvoteCommentDTO(
                request.CommentId,
                true
            ));
        }
        catch
        {
            return Result<UpvoteCommentDTO>.Failure("Failed to upvote comment.");
        }
    }

    private async Task RemoveDownvote(Guid commentId, string id)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:DOWNVOTED]->(c:Comment {id: $commentId})
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

    private async Task<bool> CommentExists(Guid commentId)
    {
        var query = @"
            MATCH (c:Comment {id: $commentId})
            RETURN c";

        var parameters = new Dictionary<string, object>{
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
