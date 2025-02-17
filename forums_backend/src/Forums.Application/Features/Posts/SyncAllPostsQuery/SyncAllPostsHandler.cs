using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Posts.SyncAllPosts;

public class SyncAllPostsHandler(IGraphDatabaseContext context) : IRequestHandler<SyncAllPostsQuery, Result<List<PostDTO>>>
{
    public async Task<Result<List<PostDTO>>> Handle(SyncAllPostsQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (f:Forum)-[:HAS_POST]->(p:Post)
            RETURN p, f";

        var resultCursor = await context.RunAsync(query);
        var result = await resultCursor.ToListAsync(cancellationToken);

        var posts = result.Select(r =>
        {
            var post = r["p"].As<INode>();
            var forum = r["f"].As<INode>();

            return new PostDTO(
                Guid.Parse(post["id"].As<string>()),
                post["title"].As<string>(),
                post["content"].As<string>(),
                new ForumDTO(
                    Guid.Parse(forum["id"].As<string>()),
                    forum["name"].As<string>())
            );
        }).ToList();

        return Result<List<PostDTO>>.Success(posts);
    }
}