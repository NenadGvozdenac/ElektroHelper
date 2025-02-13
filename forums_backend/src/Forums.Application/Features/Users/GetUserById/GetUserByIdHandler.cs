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
            return u
        ";

        var parameters = new Dictionary<string, object>
        {
            {"id", request.Id}
        };

        try {
            var resultCursor = await context.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return Result<UserByIdDTO>.Success(new UserByIdDTO(
                result["u"].As<INode>().Properties["id"].As<string>(),
                result["u"].As<INode>().Properties["username"].As<string>(),
                result["u"].As<INode>().Properties["email"].As<string>(),
                result["u"].As<INode>().Properties["role"].As<string>(),
                result["u"].As<INode>().Properties["isBanned"].As<bool>(),
                result["u"].As<INode>().Properties["reasonForBan"].As<string>(),
                result["u"].As<INode>().Properties["isDeleted"].As<bool>()
            ));
        } catch {
            return Result<UserByIdDTO>.Failure("User not found");
        }
    }
}