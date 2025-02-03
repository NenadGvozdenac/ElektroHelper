using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface IPostsService {
    public Task<PostDTO> CreatePostAsync(CreatePostDTO createPostDTO, UserDTO userDTO);
    public Task<IEnumerable<ForumAndPostsDTO>> GetMyPostsAsync(UserDTO userDTO);
    public Task<IEnumerable<PostDTO>> GetPostsAsync();
    public Task<IEnumerable<PostDTO>> GetPostsByForumIdAsync(Guid forumId);
}