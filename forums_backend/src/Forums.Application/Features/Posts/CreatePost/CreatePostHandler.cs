using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Posts.CreatePost;

public class CreatePostHandler(IGraphDatabaseContext context) : IRequestHandler<CreatePostCommand, Result<CreatedPostDTO>>
{
    public async Task<Result<CreatedPostDTO>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        if (!await ForumExists(request.CreatePostDTO.ForumId))
        {
            return Result<CreatedPostDTO>.Failure("Forum does not exist").WithCode(404);
        }

        var query = @"
            MERGE (u:User {id: $userId})
            ON CREATE SET u.email = $email, u.username = $username, u.role = $role
            WITH u
            MATCH (f:Forum)
            WHERE f.id = $forumId
            CREATE (p:Post {id: $id, title: $title, content: $content, createdAt: $createdAt, isDeleted: false, isLocked: false})
            CREATE (f)-[:HAS_POST]->(p)
            CREATE (u)-[:POSTED]->(p)
            RETURN p";

        var parameters = new Dictionary<string, object>
        {
            {"userId", request.UserDTO.Id},
            {"email", request.UserDTO.Email},
            {"username", request.UserDTO.Username},
            {"role", request.UserDTO.Role},
            {"forumId", request.CreatePostDTO.ForumId.ToString() },
            {"id", Guid.NewGuid().ToString()},
            {"title", request.CreatePostDTO.Title},
            {"content", request.CreatePostDTO.Content},
            {"createdAt", DateTime.UtcNow.ToNeo4jDateTime() }
        };

        try
        {
            var result = await context.RunAsync(query, parameters);

            var record = await result.SingleAsync();

            var post = record["p"].As<INode>();

            return Result<CreatedPostDTO>.Success(new CreatedPostDTO(
                Guid.Parse(post["id"].As<string>()),
                post["title"].As<string>(),
                post["content"].As<string>(),
                post["createdAt"].As<string>().FromNeo4jDateTime()
            ));
        }
        catch (Exception e)
        {
            return Result<CreatedPostDTO>.Failure(e.Message);
        }
    }

    private async Task<bool> ForumExists(Guid forumId)
    {
        var query = @"
            MATCH (f:Forum)
            WHERE f.id = $forumId
            RETURN f";

        var parameters = new Dictionary<string, object>
        {
            {"forumId", forumId.ToString()}
        };

        var result = await context.RunAsync(query, parameters);

        return await result.FetchAsync();
    }
}