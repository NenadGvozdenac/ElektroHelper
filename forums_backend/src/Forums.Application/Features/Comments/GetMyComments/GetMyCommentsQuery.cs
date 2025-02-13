using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Comments.GetMyComments;

public record GetMyCommentsQuery(UserDTO UserDTO) : IRequest<Result<List<CommentDTO>>>;