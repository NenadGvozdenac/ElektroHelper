using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Application.Features.Followers.GetPostsByFollowers;

public class GetPostsByFollowersHandler(IGraphDatabaseContext context) : IRequestHandler<GetPostsByFollowersQuery, Result<List<PostDTO>>>
{
    public async Task<Result<List<PostDTO>>> Handle(GetPostsByFollowersQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (u:User {id: $userId})-[:FOLLOWS]->(follower:User)-[:POSTED]->(p:Post)<-[:HAS_POST]-(f:Forum)
            OPTIONAL MATCH (u)-[upvoted:UPVOTED]->(p)
            OPTIONAL MATCH (u)-[downvoted:DOWNVOTED]->(p)
            OPTIONAL MATCH (p)<-[:HAS_COMMENT]-(c:Comment)
            OPTIONAL MATCH (u1:User)-[:UPVOTED_POST]->(p)
            OPTIONAL MATCH (u2:User)-[:DOWNVOTED_POST]->(p)
            RETURN p, f, follower, COUNT(u1) as upvotes, COUNT(u2) as downvotes, COUNT(c) as comments, 
                CASE WHEN upvoted IS NOT NULL THEN true ELSE false END as upvoted,
                CASE WHEN downvoted IS NOT NULL THEN true ELSE false END as downvoted
            ORDER BY p.createdAt DESC
        ";

        var parameters = new Dictionary<string, object>
        {
            { "userId", request.UserDTO.Id }
        };

        try
        {
            var result = await context.RunAsync(query, parameters);
            var posts = new List<PostDTO>();

            await result.ForEachAsync(record =>
            {
                var post = record["p"].As<INode>();
                var forum = record["f"].As<INode>();
                var follower = record["follower"].As<INode>();

                var upvotes = record["upvotes"].As<int>();
                var downvotes = record["downvotes"].As<int>();
                var comments = record["comments"].As<int>();
                var isUpvoted = record["upvoted"].As<bool>();
                var isDownvoted = record["downvoted"].As<bool>();

                var postDTO = new PostDTO(
                    Guid.Parse(post["id"].As<string>()),
                    post["title"].As<string>(),
                    post["content"].As<string>(),
                    post["createdAt"].As<string>().FromNeo4jDateTime(),
                    isUpvoted,
                    isDownvoted,
                    post["isDeleted"].As<bool>(),
                    upvotes,
                    downvotes,
                    comments,
                    new ForumDTO(
                        Guid.Parse(forum["id"].As<string>()),
                        forum["name"].As<string>()
                    ),
                    new AuthorDTO(
                        follower["id"].As<string>(),
                        follower["username"].As<string>(),
                        follower["email"].As<string>()
                    )
                );

                posts.Add(postDTO);
            });

            return Result<List<PostDTO>>.Success(posts);
        }
        catch (Exception ex)
        {
            return Result<List<PostDTO>>.Failure(ex.Message);
        }
    }
}
