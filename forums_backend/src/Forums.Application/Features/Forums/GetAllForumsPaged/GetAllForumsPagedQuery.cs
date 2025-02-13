using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Forums.GetAllForumsPaged;

public record GetAllForumsPagedQuery(int Page, int PageSize) : IRequest<Result<List<ForumDTO>>>;