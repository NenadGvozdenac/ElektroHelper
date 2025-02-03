using forums_backend.src.Forums.Internal.API.DTOs.Posts;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface IPostsService {
    public Task<PostDTO> CreatePostAsync(CreatePostDTO createPostDTO);
    public Task<IEnumerable<PostDTO>> GetPostsAsync();
    public Task<IEnumerable<PostDTO>> GetPostsByForumIdAsync(Guid forumId);
}