using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.GetPostsByUserIdPaged;

public record GetPostsByUserIdPagedQuery(UserDTO UserDTO, string UserId, int Page, int PageSize) : IRequest<Result<List<PostDTO>>>;