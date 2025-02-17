using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Search.SearchPosts;

public record SearchPostsQuery(string Query, int Page, int PageSize) : IRequest<Result<List<PostDTO>>>;