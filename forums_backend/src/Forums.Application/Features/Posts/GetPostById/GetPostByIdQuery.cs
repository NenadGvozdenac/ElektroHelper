using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.GetPostById;

public record GetPostByIdQuery(UserDTO UserDTO, Guid Id) : IRequest<Result<PostDTO>>;