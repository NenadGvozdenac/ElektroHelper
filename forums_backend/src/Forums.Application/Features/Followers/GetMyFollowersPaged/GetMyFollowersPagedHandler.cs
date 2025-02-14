using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Followers.GetMyFollowersPaged;

public class GetMyFollowersPagedHandler(IGraphDatabaseContext context) : IRequestHandler<GetMyFollowersPagedQuery, Result<List<FollowerDTO>>>
{
    public async Task<Result<List<FollowerDTO>>> Handle(GetMyFollowersPagedQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[:FOLLOWS]->(f:User)
            RETURN f
            SKIP $skip
            LIMIT $limit
        ";

        var parameters = new Dictionary<string, object>
        {
            { "userId", request.UserDTO.Id },
            { "skip", (request.Page - 1) * request.PageSize },
            { "limit", request.PageSize }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            var result = await resultCursor.ToListAsync();

            var followers = result.Select(r =>
            {
                var follower = r["f"].As<INode>();
                return new FollowerDTO(
                    follower["id"].As<string>(),
                    follower["email"].As<string>(),
                    follower["username"].As<string>()
                );
            }).ToList();

            return Result<List<FollowerDTO>>.Success(followers);
        }
        catch (Exception e)
        {
            return Result<List<FollowerDTO>>.Failure(e.Message);
        }
    }
}
