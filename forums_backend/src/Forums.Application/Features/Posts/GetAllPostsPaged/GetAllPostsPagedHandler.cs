using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Posts.GetAllPostsPaged;

public class GetAllPostsPagedHandler(IGraphDatabaseContext context) : IRequestHandler<GetAllPostsPagedQuery, Result<List<PostDTO>>>
{
    public async Task<Result<List<PostDTO>>> Handle(GetAllPostsPagedQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (p:Post)
            OPTIONAL MATCH (u1:User)-[upvote1:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u2:User)-[downvote1:DOWNVOTED_POST]->(p)
            OPTIONAL MATCH (c1:Comment)-[comment:BELONGS_TO]->(p)
            OPTIONAL MATCH (u4:User {id: $userId})-[upvote2:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u5:User {id: $userId})-[downvote2:DOWNVOTED_POST]->(p)
            OPTIONAL MATCH (u6:User)-[:POSTED]->(p)
            OPTIONAL MATCH (f:Forum)-[:HAS_POST]->(p)
            RETURN p, 
                count(distinct upvote1) AS upvotes, 
                count(distinct downvote1) AS downvotes, 
                count(distinct comment) AS comments,
                u6 AS author,
                CASE WHEN u4 IS NOT NULL THEN true ELSE false END AS hasUpvoted,
                CASE WHEN u5 IS NOT NULL THEN true ELSE false END AS hasDownvoted,
                f AS forum
            ORDER BY p.createdAt DESC
            SKIP $skip
            LIMIT $limit";

        var parameters = new Dictionary<string, object>
        {
            {"userId", request.UserDTO.Id},
            {"skip", (request.Page - 1) * request.PageSize },
            {"limit", request.PageSize }
        };

        try
        {
            var result = await context.RunAsync(query, parameters);
            var posts = await result.ToListAsync(record =>
            {
                var post = record["p"].As<INode>();
                var upvotes = record["upvotes"].As<int>();
                var downvotes = record["downvotes"].As<int>();
                var comments = record["comments"].As<int>();
                var author = record["author"].As<INode>();
                var hasUpvoted = record["hasUpvoted"].As<bool>();
                var hasDownvoted = record["hasDownvoted"].As<bool>();
                var forum = record["forum"].As<INode>();

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
                    ),
                    new ForumDTO(Guid.Parse(forum["id"].As<string>()), forum["name"].As<string>())
                );
            });

            return Result<List<PostDTO>>.Success(posts);
        }
        catch (Exception)
        {
            return Result<List<PostDTO>>.Failure("Failed to get posts");
        }
    }
}