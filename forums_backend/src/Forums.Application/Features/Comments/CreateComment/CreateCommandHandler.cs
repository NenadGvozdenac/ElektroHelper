using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Comments.CreateComment;

public class CreateCommandHandler(IGraphDatabaseContext context) : IRequestHandler<CreateCommentCommand, Result<CreatedCommentDTO>>
{
    public async Task<Result<CreatedCommentDTO>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        if(!await PostExists(request.PostId)) {
            return Result<CreatedCommentDTO>.Failure("Post does not exist").WithCode(404);
        }

        var query = @"MATCH (p:Post {id: $postId}), (u:User {id: $userId})
            CREATE (c:Comment {id: $id, content: $content, createdAt: $createdAt, isDeleted: false})
            CREATE (u)-[:COMMENTED]->(c)-[:BELONGS_TO]->(p)
            RETURN c";

        var parameters = new Dictionary<string, object> {
            { "id", Guid.NewGuid().ToString() },
            { "content", request.Content },
            { "createdAt", DateTime.UtcNow.ToNeo4jDateTime() },
            { "postId", request.PostId.ToString() },
            { "userId", request.UserDTO.Id.ToString() }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            return Result<CreatedCommentDTO>.Success(new CreatedCommentDTO(
                Guid.Parse(result["c"].As<INode>().Properties["id"].As<string>()),
                result["c"].As<INode>().Properties["content"].As<string>(),
                result["c"].As<INode>().Properties["createdAt"].As<string>().FromNeo4jDateTime()
            ));
        }
        catch
        {
            return Result<CreatedCommentDTO>.Failure("Failed to create comment").WithCode(500);
        }
    }

    private async Task<bool> PostExists(Guid postId)
    {
        var query = @"MATCH (p:Post {id: $postId}) RETURN p";

        var parameters = new Dictionary<string, object> {
            { "postId", postId.ToString() }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        return await resultCursor.FetchAsync();
    }
}