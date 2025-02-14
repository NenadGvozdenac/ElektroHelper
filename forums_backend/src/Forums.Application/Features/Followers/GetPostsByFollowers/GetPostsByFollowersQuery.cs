using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Followers.GetPostsByFollowers;

public record GetPostsByFollowersQuery(UserDTO UserDTO) : IRequest<Result<List<PostDTO>>>;