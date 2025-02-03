using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class PostsService : IPostsService
{
    private readonly IPostsRepository _postsRepository;

    public PostsService(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public async Task<PostDTO> CreatePostAsync(CreatePostDTO createPostDTO, UserDTO userDTO)
    {
        Post post = new(createPostDTO.Title, createPostDTO.Content);
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        await _postsRepository.AddAsync(post, createPostDTO.ForumId, user);

        return new PostDTO(post.Id, post.Title, post.Content, post.CreatedAt);
    }

    public async Task<IEnumerable<ForumAndPostsDTO>> GetMyPostsAsync(UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var forumsAndPosts = await _postsRepository.GetMyForumsAndPostsAsync(user);

        return forumsAndPosts.Select(forumAndPosts => 
            new ForumAndPostsDTO(forumAndPosts.Forum.Id, 
            forumAndPosts.Forum.Name, 
            forumAndPosts.Forum.Description, 
            forumAndPosts.Forum.CreatedAt,
            forumAndPosts.Posts.Select(post => 
                new PostDTO(post.Id, post.Title, post.Content, post.CreatedAt))));
    }

    public async Task<IEnumerable<PostDTO>> GetPostsAsync()
    {
        var posts = await _postsRepository.GetAllAsync();

        return posts.Select(post => new PostDTO(post.Id, post.Title, post.Content, post.CreatedAt));
    }

    public async Task<IEnumerable<PostDTO>> GetPostsByForumIdAsync(Guid forumId)
    {
        var posts = await _postsRepository.GetPostsByForumIdAsync(forumId);

        return posts.Select(post => new PostDTO(post.Id, post.Title, post.Content, post.CreatedAt));
    }
}