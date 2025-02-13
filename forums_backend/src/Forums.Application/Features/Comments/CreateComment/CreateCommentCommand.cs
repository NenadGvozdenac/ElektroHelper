using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Comments.CreateComment;

public record CreateCommentCommand(UserDTO UserDTO, string Content, Guid PostId) : IRequest<Result<CreatedCommentDTO>>;