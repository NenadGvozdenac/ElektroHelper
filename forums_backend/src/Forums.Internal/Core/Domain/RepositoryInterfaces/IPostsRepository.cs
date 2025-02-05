using forums_backend.src.Forums.BuildingBlocks.Core.UseCases;

namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IPostsRepository
{
    public Task<Post?> AddAsync(Post post, Guid forumId, User user);
    public Task<IEnumerable<Post>> GetAllAsync();
    public Task<Post?> GetByIdAsync(Guid postId);
    public Task<IEnumerable<ForumAndPosts>> GetMyForumsAndPostsAsync(User user);
    public Task<IEnumerable<Post>> GetPostsByForumIdAsync(Guid forumId);
}
