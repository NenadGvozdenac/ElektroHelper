using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Followers.GetPostsByFollowersPaged;

public record GetPostsByFollowersPagedQuery(UserDTO UserDTO, int Page, int PageSize) : IRequest<Result<List<PostDTO>>>;