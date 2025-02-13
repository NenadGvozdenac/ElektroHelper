using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Comments.RemoveCommentUpvote;

public record RemoveCommentUpvoteCommand(UserDTO UserDTO, Guid CommentId) : IRequest<Result<CommentUpvoteDTO>>;