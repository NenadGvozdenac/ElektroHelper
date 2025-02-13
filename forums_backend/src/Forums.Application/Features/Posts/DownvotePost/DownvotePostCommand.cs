using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.DownvotePost;

public record DownvotePostCommand(UserDTO UserDTO, Guid PostId) : IRequest<Result<DownvotePostDTO>>;