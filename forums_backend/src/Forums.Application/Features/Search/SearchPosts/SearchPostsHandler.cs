using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Search.SearchPosts;

public class SearchPostsHandler(IVectorDatabaseContext context) : IRequestHandler<SearchPostsQuery, Result<List<PostDTO>>>
{
    public async Task<Result<List<PostDTO>>> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
    {
        var searchResult = await context.QueryData<PostDTO>(request.Query, "forum_posts");

        var result = searchResult.Documents.ToList().Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

        return Result<List<PostDTO>>.Success(result);
    }
}
