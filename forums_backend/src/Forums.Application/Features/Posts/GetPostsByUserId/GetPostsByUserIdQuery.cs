using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.GetPostsByUserId;

public record GetPostsByUserIdQuery(UserDTO UserDTO, string UserId) : IRequest<Result<List<PostDTO>>>;