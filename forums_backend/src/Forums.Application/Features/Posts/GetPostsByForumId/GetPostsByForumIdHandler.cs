using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Posts.GetPostsByForumId;

public class GetPostsByForumIdHandler(IGraphDatabaseContext context) : IRequestHandler<GetPostsByForumIdQuery, Result<List<PostDTO>>>
{
    public async Task<Result<List<PostDTO>>> Handle(GetPostsByForumIdQuery request, CancellationToken cancellationToken)
    {
        var query = @"
                MATCH (f:Forum {id: $forumId})-[:HAS_POST]->(p:Post)
                OPTIONAL MATCH (u1:User)-[upvote:UPVOTED_POST]->(p)
                OPTIONAL MATCH (u2:User)-[downvote:DOWNVOTED_POST]->(p)
                OPTIONAL MATCH (c:Comment)-[comment:BELONGS_TO]->(p)
                OPTIONAL MATCH (u4:User {id: $userId})-[:UPVOTED_POST]->(p)
                OPTIONAL MATCH (u5:User {id: $userId})-[:DOWNVOTED_POST]->(p)
                OPTIONAL MATCH (u6:User)-[:POSTED]->(p)
                OPTIONAL MATCH (f:Forum)-[:HAS_POST]->(p)
                RETURN p, count(distinct upvote) AS upvotes, count(distinct downvote) AS downvotes, count(distinct comment) AS comments,
                    u6 AS author, f AS forum,
                    CASE WHEN u4 IS NOT NULL THEN true ELSE false END AS hasUpvoted,
                    CASE WHEN u5 IS NOT NULL THEN true ELSE false END AS hasDownvoted";

        var parameters = new Dictionary<string, object> {
            { "forumId", request.ForumId.ToString() } ,
            { "userId", request.UserDTO.Id }
        };

        var resultCursor = await context.RunAsync(query, parameters);
        var result = await resultCursor.ToListAsync(record =>
        {
            var post = record["p"].As<INode>();
            var author = record["author"].As<INode>();
            var forum = record["forum"].As<INode>();
            var upvotes = record["upvotes"].As<int>();
            var downvotes = record["downvotes"].As<int>();
            var comments = record["comments"].As<int>();
            var hasUpvoted = record["hasUpvoted"].As<bool>();
            var hasDownvoted = record["hasDownvoted"].As<bool>();

            return new PostDTO(
                Guid.Parse(post["id"].As<string>()),
                post["title"].As<string>(),
                post["content"].As<string>(),
                post["createdAt"].As<string>().FromNeo4jDateTime(),
                hasUpvoted,
                hasDownvoted,
                upvotes,
                downvotes,
                comments,
                new AuthorDTO(
                    author["id"].As<string>(),
                    author["username"].As<string>(),
                    author["email"].As<string>()
                )
            );
        });

        return Result<List<PostDTO>>.Success(result);
    }
}