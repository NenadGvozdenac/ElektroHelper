using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Comments.RemoveCommentDownvote;

public record RemoveCommentDownvoteCommand(UserDTO UserDTO, Guid CommentId) : IRequest<Result<CommentDownvoteDTO>>;