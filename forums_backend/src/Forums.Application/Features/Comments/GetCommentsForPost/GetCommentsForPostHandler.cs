using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;
using Neo4j.Driver.Mapping;

namespace forums_backend.src.Forums.Application.Features.Comments.GetCommentsForPost;

public class GetCommentsForPostHandler(IGraphDatabaseContext context) : IRequestHandler<GetCommentsForPostQuery, Result<List<CommentDTO>>>
{
    public async Task<Result<List<CommentDTO>>> Handle(GetCommentsForPostQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (p:Post {id: $postId})<-[:BELONGS_TO]-(c:Comment)
            OPTIONAL MATCH (u1:User {id: $userId})-[uv:UPVOTED]->(c)
            OPTIONAL MATCH (u2:User {id: $userId})-[dv:DOWNVOTED]->(c)
            OPTIONAL MATCH (u3)-[uv1:UPVOTED]->(c)
            OPTIONAL MATCH (u4)-[dv1:DOWNVOTED]->(c)
            OPTIONAL MATCH (u5: User)-[:COMMENTED]->(c)
            RETURN c, COUNT(distinct uv1) as upvotes, COUNT(distinct dv1) as downvotes, u5 as author,
                CASE WHEN COUNT(distinct uv) > 0 THEN true ELSE false END as upvoted,
                CASE WHEN COUNT(distinct dv) > 0 THEN true ELSE false END as downvoted";

        var parameters = new Dictionary<string, object> {
            { "postId", request.PostId.ToString() },
            { "userId", request.UserDTO.Id }
        };

        try
        {
            var resultCursor = await context.RunAsync(query, parameters);

            var comments = await resultCursor.ToListAsync(record =>
            {
                var comment = record["c"].As<INode>();
                var author = record["author"].As<INode>();
                var upvotes = record["upvotes"].As<int>();
                var downvotes = record["downvotes"].As<int>();
                var upvoted = record["upvoted"].As<bool>();
                var downvoted = record["downvoted"].As<bool>();

                return new CommentDTO(
                    Guid.Parse(comment["id"].As<string>()),
                    comment["content"].As<string>(),
                    new AuthorDTO(
                        author["id"].As<string>(),
                        author["email"].As<string>(),
                        author["role"].As<string>(),
                        author["username"].As<string>()
                    ),
                    comment["createdAt"].As<string>().FromNeo4jDateTime(),
                    upvotes,
                    downvotes,
                    upvoted,
                    downvoted,
                    comment["isDeleted"].As<bool>()
                );
            });

            return Result<List<CommentDTO>>.Success(comments);
        }
        catch (Exception e)
        {
            return Result<List<CommentDTO>>.Failure(e.Message).WithCode(500);
        }
    }
}