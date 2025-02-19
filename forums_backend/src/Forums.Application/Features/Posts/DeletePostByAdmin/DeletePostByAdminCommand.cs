using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.DeletePostByAdmin;

public record DeletePostByAdminCommand(UserDTO UserDTO, Guid PostId) : IRequest<Result<DeletePostDTO>>;