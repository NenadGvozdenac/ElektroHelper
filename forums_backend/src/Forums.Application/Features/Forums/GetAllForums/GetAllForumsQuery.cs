using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Forums.GetAllForums;

public record GetAllForumsQuery : IRequest<Result<List<ForumDTO>>>;