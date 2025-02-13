using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Forums.GetAllForums;

public class GetAllForumsHandler(IGraphDatabaseContext context) : IRequestHandler<GetAllForumsQuery, Result<List<ForumDTO>>>
{
    public async Task<Result<List<ForumDTO>>> Handle(GetAllForumsQuery request, CancellationToken cancellationToken)
    {
        var forums = new List<ForumDTO>();

        var query = @"
            MATCH (forum:Forum)
            OPTIONAL MATCH (forum)-[:HAS_POST]->(post:Post)
            RETURN forum, COUNT(post) as numberOfPosts";

        var resultCursor = await context.RunAsync(query);
        var results = await resultCursor.ToListAsync(cancellationToken);

        foreach (var result in results)
        {
            var forum = result["forum"].As<INode>();
            var numberOfPosts = result["numberOfPosts"].As<int>();

            forums.Add(new ForumDTO(
                Guid.Parse(forum["id"].As<string>()),
                forum["name"].As<string>(),
                forum["description"].As<string>(),
                numberOfPosts
            ));
        }

        return Result<List<ForumDTO>>.Success(forums);
    }
}