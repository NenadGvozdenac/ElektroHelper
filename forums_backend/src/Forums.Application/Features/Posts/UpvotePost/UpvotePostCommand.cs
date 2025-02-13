using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.UpvotePost;

public record UpvotePostCommand(UserDTO UserDTO, Guid PostId) : IRequest<Result<UpvotePostDTO>>;