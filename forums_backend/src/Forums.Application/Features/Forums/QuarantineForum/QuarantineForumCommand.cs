using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Forums.QuarantineForum;

public record QuarantineForumCommand(UserDTO UserDTO, Guid ForumId) : IRequest<Result<QuarantineForumDTO>>;