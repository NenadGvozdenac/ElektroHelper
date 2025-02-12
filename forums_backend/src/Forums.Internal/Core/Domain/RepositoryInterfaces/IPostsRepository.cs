using forums_backend.src.Forums.BuildingBlocks.Core.UseCases;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IPostsRepository
{
    public Task<Post?> AddAsync(Post post, Guid forumId, UserDTO userDTO);
    public Task<IEnumerable<Post>> GetAllAsync();
    public Task<PostVoting?> GetByIdAsync(Guid postId, UserDTO userDTO);
    public Task<IEnumerable<ForumAndPosts>> GetMyForumsAndPostsAsync(UserDTO userDTO);
    public Task<IEnumerable<PostVoting>> GetPagedAsync(int page, int pageSize, UserDTO userDTO);
    public Task<IEnumerable<PostVoting>> GetPostsByForumIdAsync(Guid forumId, UserDTO userDTO);
    public Task<IEnumerable<PostVoting>> GetPostsByForumIdPagedAsync(int page, int pageSize, Guid forumId, UserDTO userDTO);
}
