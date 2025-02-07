using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface IPostsService {
    public Task<Result<PostDTO>> CreatePostAsync(CreatePostDTO createPostDTO, UserDTO userDTO);
    public Task<Result<IEnumerable<ForumAndPostsDTO>>> GetMyPostsAsync(UserDTO userDTO);
    public Task<Result<IEnumerable<PostDTO>>> GetPostsAsync();
    public Task<Result<IEnumerable<PostDTO>>> GetPostsAsync(int page, int pageSize, UserDTO userDTO);
    public Task<Result<IEnumerable<PostDTO>>> GetPostsByForumIdAsync(Guid forumId);
}