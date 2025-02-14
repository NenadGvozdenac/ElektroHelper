using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Forums.QuarantineForum;

public class QuarantineForumHandler(IGraphDatabaseContext context) : IRequestHandler<QuarantineForumCommand, Result<QuarantineForumDTO>>
{
    public async Task<Result<QuarantineForumDTO>> Handle(QuarantineForumCommand request, CancellationToken cancellationToken)
    {
        if(request.UserDTO.Role != "admin") {
            return Result<QuarantineForumDTO>.Failure("User is not an admin").WithCode(403);
        }

        if (!await ForumExists(request.ForumId))
        {
            return Result<QuarantineForumDTO>.Failure("Forum does not exist").WithCode(404);
        }

        if(await IsForumQuarantined(request.ForumId))
        {
            return Result<QuarantineForumDTO>.Failure("Forum is already quarantined").WithCode(400);
        }

        var query = @"
            MATCH (f:Forum {id: $forumId})
            SET f.isQuarantined = true
            RETURN f
        ";

        var parameters = new Dictionary<string, object> {
            {"forumId", request.ForumId.ToString() },
            {"userId", request.UserDTO.Id}
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            return Result<QuarantineForumDTO>.Success(new QuarantineForumDTO(request.ForumId, true));
        } catch(Exception e) {
            return Result<QuarantineForumDTO>.Failure(e.Message);
        }
    }

    private async Task<bool> IsForumQuarantined(Guid forumId)
    {
        var query = @"
            MATCH (f:Forum {id: $forumId})
            WHERE f.isQuarantined = true
            RETURN f
        ";

        var parameters = new Dictionary<string, object> {
            {"forumId", forumId.ToString()}
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }

    private async Task<bool> ForumExists(Guid forumId)
    {
        var query = @"
            MATCH (f:Forum {id: $forumId})
            RETURN f
        ";

        var parameters = new Dictionary<string, object> {
            {"forumId", forumId.ToString()}
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }
}