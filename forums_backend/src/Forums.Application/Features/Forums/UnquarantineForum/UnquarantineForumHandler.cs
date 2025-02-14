using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Forums.UnquarantineForum;

public class UnquarantineForumHandler(IGraphDatabaseContext context) : IRequestHandler<UnquarantineForumCommand, Result<UnquarantineForumDTO>>
{
    public async Task<Result<UnquarantineForumDTO>> Handle(UnquarantineForumCommand request, CancellationToken cancellationToken)
    {
        if(request.UserDTO.Role != "admin") {
            return Result<UnquarantineForumDTO>.Failure("User is not an admin").WithCode(403);
        }

        if (!await ForumExists(request.ForumId))
        {
            return Result<UnquarantineForumDTO>.Failure("Forum does not exist").WithCode(404);
        }

        if(!await IsForumQuarantined(request.ForumId))
        {
            return Result<UnquarantineForumDTO>.Failure("Forum is not quarantined").WithCode(400);
        }

        var query = @"
            MATCH (f:Forum {id: $forumId})
            SET f.isQuarantined = false
            RETURN f";
        
        var parameters = new Dictionary<string, object> {
            {"forumId", request.ForumId.ToString() },
            {"userId", request.UserDTO.Id}
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            return Result<UnquarantineForumDTO>.Success(new UnquarantineForumDTO(request.ForumId, false));
        } catch(Exception e) {
            return Result<UnquarantineForumDTO>.Failure(e.Message);
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
