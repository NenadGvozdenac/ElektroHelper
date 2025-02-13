using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.RemovePostDownvote;

public record RemovePostDownvoteCommand(UserDTO UserDTO, Guid PostId) : IRequest<Result<PostDownvoteDTO>>;