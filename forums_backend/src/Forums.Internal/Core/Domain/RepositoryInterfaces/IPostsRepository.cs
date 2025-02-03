using forums_backend.src.Forums.BuildingBlocks.Core.UseCases;

namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IPostsRepository
{
    public Task<Post> AddAsync(Post post, Guid forumId);
    public Task<IEnumerable<Post>> GetAllAsync();
    public Task<IEnumerable<Post>> GetPostsByForumIdAsync(Guid forumId);
}
