using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Users.CreateUser;

public class CreateUserHandler(IGraphDatabaseContext context) : IRequestHandler<CreateUserCommand, Result<CreatedUserDTO>>
{
    public async Task<Result<CreatedUserDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var query = @"
            MERGE (user:User {id: $id})
            ON CREATE SET 
                user.email = $email,
                user.username = $username,
                user.role = $role,
                user.isBanned = false,
                user.reasonForBan = '',
                user.isDeleted = false
            RETURN user
        ";

        var parameters = new Dictionary<string, object>
        {
            { "id", request.CreateUserDTO.Id },
            { "email", request.CreateUserDTO.Email },
            { "username", request.CreateUserDTO.Username },
            { "role", request.CreateUserDTO.Role },
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return Result<CreatedUserDTO>.Success(new CreatedUserDTO(
                result["user"].As<INode>().Properties["id"].As<string>(),
                result["user"].As<INode>().Properties["username"].As<string>(),
                result["user"].As<INode>().Properties["email"].As<string>(),
                result["user"].As<INode>().Properties["role"].As<string>()
            ));
        }
        catch
        {
            return Result<CreatedUserDTO>.Failure("Failed to create user.");
        }
    }
}