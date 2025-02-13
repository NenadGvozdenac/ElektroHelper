using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Comments.GetCommentsForPost;

public record GetCommentsForPostQuery(UserDTO UserDTO, Guid PostId)
    : IRequest<Result<List<CommentDTO>>>;