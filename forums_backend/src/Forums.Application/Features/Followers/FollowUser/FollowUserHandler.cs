using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Followers.FollowUser;

public record FollowUserHandler(IGraphDatabaseContext context) : IRequestHandler<FollowUserCommand, Result<FollowUserDTO>>
{
    public async Task<Result<FollowUserDTO>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        if(request.FollowerId == request.UserDTO.Id) {
            return Result<FollowUserDTO>.Failure("User cannot follow themselves");
        }

        if(!await UserExists(request.FollowerId)) {
            return Result<FollowUserDTO>.Failure("Follower does not exist");
        }

        if(await UserAlreadyFollows(request.UserDTO.Id, request.FollowerId)) {
            return Result<FollowUserDTO>.Failure("User already follows this user");
        }

        var query = @"
            MATCH (u:User {id: $userId})
            MATCH (f:User {id: $followerId})
            MERGE (u)-[r:FOLLOWS]->(f)
            RETURN u.id as UserId, f.id as FollowerId
        ";

        var parameters = new Dictionary<string, object>
        {
            { "userId", request.UserDTO.Id },
            { "followerId", request.FollowerId }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return Result<FollowUserDTO>.Success(
                new FollowUserDTO(result["UserId"].As<string>(), result["FollowerId"].As<string>(), true)
            );
        }
        catch (Exception e)
        {
            return Result<FollowUserDTO>.Failure(e.Message);
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
            MATCH (u:User {id: $followerId})
            RETURN u";
        
        var parameters = new Dictionary<string, object>
        {
            { "followerId", followerId }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }
}