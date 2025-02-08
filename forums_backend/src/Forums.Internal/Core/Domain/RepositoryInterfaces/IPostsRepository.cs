using forums_backend.src.Forums.BuildingBlocks.Core.UseCases;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IPostsRepository
{
    public Task<Post?> AddAsync(Post post, Guid forumId, User user);
    public Task<IEnumerable<Post>> GetAllAsync();
    public Task<Post?> GetByIdAsync(Guid postId);
    public Task<IEnumerable<ForumAndPosts>> GetMyForumsAndPostsAsync(User user);
    public Task<IEnumerable<PostVoting>> GetPagedAsync(int page, int pageSize, UserDTO userDTO);
    public Task<IEnumerable<PostVoting>> GetPostsByForumIdAsync(Guid forumId, UserDTO userDTO);
    public Task<IEnumerable<PostVoting>> GetPostsByForumIdPagedAsync(int page, int pageSize, Guid forumId, UserDTO userDTO);
}
