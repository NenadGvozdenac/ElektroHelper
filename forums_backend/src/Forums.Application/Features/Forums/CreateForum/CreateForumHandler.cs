using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Forums.CreateForum;

public class CreateForumHandler(IGraphDatabaseContext context) : IRequestHandler<CreateForumCommand, Result<CreatedForumDTO>>
{
    public async Task<Result<CreatedForumDTO>> Handle(CreateForumCommand request, CancellationToken cancellationToken)
    {
        var query = @"
            MERGE (u:User { id: $userId })
            ON CREATE SET u.email = $userEmail, u.username = $username, u.role = $userRole
            CREATE (forum:Forum {
                id: $id,
                name: $name,
                description: $description,
                createdAt: $createdAt,
                isDeleted: false,
                isQuarantined: false
            })
            CREATE (u)-[:CREATED]->(forum)
            RETURN forum
        ";

        var parameters = new Dictionary<string, object>
        {
            { "id", Guid.NewGuid().ToString() },
            { "name", request.Name },
            { "description", request.Description },
            { "userId", request.UserDTO.Id },
            { "userEmail", request.UserDTO.Email },
            { "username", request.UserDTO.Username },
            { "userRole", request.UserDTO.Role },
            { "createdAt", DateTime.UtcNow.ToNeo4jDateTime() }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            var forum = result["forum"].As<INode>();
            return Result<CreatedForumDTO>.Success(new CreatedForumDTO(
                Guid.Parse(forum["id"].As<string>()),
                forum["name"].As<string>(),
                forum["description"].As<string>()
            ));
        }
        catch
        {
            return Result<CreatedForumDTO>.Failure("Failed to create forum.");
        }
    }
}