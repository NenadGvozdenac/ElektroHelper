using MediatR;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using Neo4j.Driver;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;

namespace forums_backend.src.Forums.Application.Features.Forums.GetAllForumsPaged;

public class GetAllForumsPagedHandler(IGraphDatabaseContext context) : IRequestHandler<GetAllForumsPagedQuery, Result<List<ForumDTO>>>
{
    public async Task<Result<List<ForumDTO>>> Handle(GetAllForumsPagedQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (forum:Forum)
            OPTIONAL MATCH (forum)-[:HAS_POST]->(post:Post)
            RETURN forum, COUNT(post) as numberOfPosts
            SKIP $skip
            LIMIT $limit";

        var parameters = new Dictionary<string, object>
        {
            { "skip", (request.Page - 1) * request.PageSize },
            { "limit", request.PageSize }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        var results = await resultCursor.ToListAsync(cancellationToken);

        var result = results.Select(result =>
        {
            var forum = result["forum"].As<INode>();
            var numberOfPosts = result["numberOfPosts"].As<int>();

            return new ForumDTO(
                Guid.Parse(forum["id"].As<string>()),
                forum["name"].As<string>(),
                forum["description"].As<string>(),
                numberOfPosts
            );
        }).ToList();

        return Result<List<ForumDTO>>.Success(result);
    }
}