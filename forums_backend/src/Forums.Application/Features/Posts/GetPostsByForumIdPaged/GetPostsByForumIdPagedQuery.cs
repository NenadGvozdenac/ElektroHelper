using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.GetPostsByForumIdPaged;

public record GetPostsByForumIdPagedQuery(UserDTO UserDTO, Guid ForumId, int Page, int PageSize) : IRequest<Result<List<PostDTO>>>;