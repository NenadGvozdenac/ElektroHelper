using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Forums.GetForumById;

public class GetForumByIdHandler(IGraphDatabaseContext context) : IRequestHandler<GetForumByIdQuery, Result<ForumDTO>>
{
    public async Task<Result<ForumDTO>> Handle(GetForumByIdQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (forum:Forum { id: $id })
            OPTIONAL MATCH (forum)-[:HAS_POST]->(post:Post)
            RETURN forum, COUNT(post) as numberOfPosts";

        var parameters = new Dictionary<string, object>
        {
            { "id", request.Id.ToString() }
        };

        try {
            var resultCursor = await context.RunAsync(query, parameters);
            var result = await resultCursor.SingleAsync();

            var forum = result["forum"].As<INode>();
            var numberOfPosts = result["numberOfPosts"].As<int>();

            return Result<ForumDTO>.Success(new ForumDTO(
                Guid.Parse(forum["id"].As<string>()),
                forum["name"].As<string>(),
                forum["description"].As<string>(),
                numberOfPosts
            ));
        } catch {
            return Result<ForumDTO>.Failure("Failed to get forum");
        }
    }
}