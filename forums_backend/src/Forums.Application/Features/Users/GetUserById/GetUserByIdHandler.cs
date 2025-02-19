using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Users.GetUserById;

public class GetUserByIdHandler(IGraphDatabaseContext context) : IRequestHandler<GetUserByIdQuery, Result<UserByIdDTO>>
{
    public async Task<Result<UserByIdDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (u:User {id: $id})
            OPTIONAL MATCH (:User)-[follows:FOLLOWS]->(u)
            OPTIONAL MATCH (u)-[follows2:FOLLOWS]->(:User)
            OPTIONAL MATCH (u)-[posted:POSTED]->(post:Post)
            WHERE post.isDeleted = false
            OPTIONAL MATCH (u)-[commented:COMMENTED]->(:Comment)
            OPTIONAL MATCH (u1: User{ id: $id2 })-[follows3:FOLLOWS]->(u)
            OPTIONAL MATCH (u1: User{ id: $id2 })-[blocked:BLOCKED]->(u)
            RETURN u,
                count(distinct follows) as followers,
                count(distinct follows2) as following,
                count(distinct posted) as numberOfPosts,
                count(distinct commented) as numberOfComments,
                CASE WHEN follows3 IS NOT NULL THEN true ELSE false END as isFollowed,
                CASE WHEN blocked IS NOT NULL THEN true ELSE false END as isBlocked";

        var parameters = new Dictionary<string, object>
        {
            {"id", request.Id },
            {"id2", request.UserDTO.Id }
        };

        try {
            var resultCursor = await context.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            var user = result["u"].As<INode>();

            return Result<UserByIdDTO>.Success(new UserByIdDTO(
                user["id"].As<string>(),
                user["username"].As<string>(),
                user["email"].As<string>(),
                user["role"].As<string>(),
                user["isBanned"].As<bool>(),
                user["reasonForBan"].As<string>(),
                result["isFollowed"].As<bool>(),
                result["isBlocked"].As<bool>(),
                user["isDeleted"].As<bool>(),
                result["numberOfPosts"].As<int>(),
                result["numberOfComments"].As<int>(),
                result["followers"].As<int>(),
                result["following"].As<int>()
            ));
        } catch(Exception e) {
            return Result<UserByIdDTO>.Failure(e.Message);
        }
    }
}