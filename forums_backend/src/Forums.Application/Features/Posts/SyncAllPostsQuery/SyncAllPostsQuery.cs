using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.SyncAllPosts;

public record SyncAllPostsQuery() : IRequest<Result<List<PostDTO>>>;