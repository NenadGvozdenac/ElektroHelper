using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Followers.GetMyFollowersPaged;

public record GetMyFollowersPagedQuery(UserDTO UserDTO, int Page, int PageSize) : IRequest<Result<List<FollowerDTO>>>;