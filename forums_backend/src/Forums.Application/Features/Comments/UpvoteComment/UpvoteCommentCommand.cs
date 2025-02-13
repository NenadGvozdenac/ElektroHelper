using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Comments.UpvoteComment;

public record UpvoteCommentCommand(UserDTO UserDTO, Guid CommentId) : IRequest<Result<UpvoteCommentDTO>>;