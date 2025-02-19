using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Posts.GetPostById;

public class GetPostByIdHandler(IGraphDatabaseContext context) : IRequestHandler<GetPostByIdQuery, Result<PostDTO>>
{
    public async Task<Result<PostDTO>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (p:Post)
            WHERE p.id = $postId AND p.isDeleted = false
            OPTIONAL MATCH (u1:User)-[upvote:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u2:User)-[downvote:DOWNVOTED_POST]->(p)
            OPTIONAL MATCH (f:Forum)-[:HAS_POST]->(p)
            OPTIONAL MATCH (u3:User)-[:POSTED]->(p)
            OPTIONAL MATCH (c:Comment)-[:BELONGS_TO]->(p)
            OPTIONAL MATCH (u4:User {id: $userId})-[:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u5:User {id: $userId})-[:DOWNVOTED_POST]->(p)
            RETURN p, count(distinct upvote) AS upvotes, count(distinct downvote) AS downvotes, count(distinct c) AS comments, f AS forum,
                u3 AS author,
                CASE WHEN u4 IS NOT NULL THEN true ELSE false END AS hasUpvoted,
                CASE WHEN u5 IS NOT NULL THEN true ELSE false END AS hasDownvoted";

        var parameters = new Dictionary<string, object>
        {
            { "postId", request.Id.ToString() },
            { "userId", request.UserDTO.Id }
        };

        try
        {
            var result = await context.RunAsync(query, parameters);
            var record = await result.SingleAsync();

            var post = record["p"].As<INode>();
            var upvotes = record["upvotes"].As<int>();
            var downvotes = record["downvotes"].As<int>();
            var comments = record["comments"].As<int>();
            var forum = record["forum"].As<INode>();
            var author = record["author"].As<INode>();
            var hasUpvoted = record["hasUpvoted"].As<bool>();
            var hasDownvoted = record["hasDownvoted"].As<bool>();

            return Result<PostDTO>.Success(new PostDTO(
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
                ),
                new ForumDTO(
                    Guid.Parse(forum["id"].As<string>()),
                    forum["name"].As<string>()
                )
            ));
        }
        catch (Exception e)
        {
            return Result<PostDTO>.Failure(e.Message);
        }
    }
}