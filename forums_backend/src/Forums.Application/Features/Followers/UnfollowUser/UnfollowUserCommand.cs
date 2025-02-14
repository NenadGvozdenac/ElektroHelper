using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Followers.UnfollowUser;

public record UnfollowUserCommand(UserDTO UserDTO, string FollowerId) : IRequest<Result<UnfollowUserDTO>>;