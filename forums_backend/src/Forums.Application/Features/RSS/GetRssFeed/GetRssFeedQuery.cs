using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.RSS.GetRssFeed;

public record GetRssFeedQuery : IRequest<Result<string>>;