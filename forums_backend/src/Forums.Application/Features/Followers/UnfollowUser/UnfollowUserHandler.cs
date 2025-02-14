using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Followers.UnfollowUser;

public class UnfollowUserHandler(IGraphDatabaseContext context) : IRequestHandler<UnfollowUserCommand, Result<UnfollowUserDTO>>
{
    public async Task<Result<UnfollowUserDTO>> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
    {
        if(request.FollowerId == request.UserDTO.Id) {
            return Result<UnfollowUserDTO>.Failure("User cannot unfollow themselves");
        }

        if(!await UserExists(request.FollowerId)) {
            return Result<UnfollowUserDTO>.Failure("Follower does not exist");
        }

        if(!await UserAlreadyFollows(request.UserDTO.Id, request.FollowerId)) {
            return Result<UnfollowUserDTO>.Failure("User does not follow this user");
        }

        var query = @"
            MATCH (u:User {id: $userId})-[r:FOLLOWS]->(f:User {id: $followerId})
            DELETE r
            RETURN u.id as UserId, f.id as FollowerId";

        var parameters = new Dictionary<string, object>
        {
            { "userId", request.UserDTO.Id },
            { "followerId", request.FollowerId }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return Result<UnfollowUserDTO>.Success(
                new UnfollowUserDTO(result["UserId"].As<string>(), result["FollowerId"].As<string>(), false)
            );
        }
        catch (Exception e)
        {
            return Result<UnfollowUserDTO>.Failure(e.Message);
        }
    }

    private async Task<bool> UserAlreadyFollows(string id, string followerId)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[r:FOLLOWS]->(f:User {id: $followerId})
            RETURN r";
        
        var parameters = new Dictionary<string, object>
        {
            { "userId", id },
            { "followerId", followerId }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }

    private async Task<bool> UserExists(string followerId)
    {
        var query = @"
            MATCH (u:User {id: $userId})
            RETURN u";
        
        var parameters = new Dictionary<string, object>
        {
            { "userId", followerId }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }
}
