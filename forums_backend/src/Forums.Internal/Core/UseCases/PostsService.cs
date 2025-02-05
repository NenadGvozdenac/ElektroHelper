using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class PostsService : IPostsService
{
    private readonly IPostsRepository _postsRepository;
    private readonly IForumsRepository _forumsRepository;

    public PostsService(IPostsRepository postsRepository, IForumsRepository forumsRepository)
    {
        _postsRepository = postsRepository;
        _forumsRepository = forumsRepository;
    }

    public async Task<Result<PostDTO>> CreatePostAsync(CreatePostDTO createPostDTO, UserDTO userDTO)
    {
        Post post = new(createPostDTO.Title, createPostDTO.Content);
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        Forum? forum = await _forumsRepository.GetByIdAsync(createPostDTO.ForumId);

        if (forum == null)
        {
            return Result<PostDTO>.Failure("Forum not found").WithCode(404);
        }

        Post? addedPost = await _postsRepository.AddAsync(post, createPostDTO.ForumId, user);

        if (addedPost == null)
        {
            return Result<PostDTO>.Failure("Failed to create post");
        }

        return Result<PostDTO>.Success(new PostDTO(post.Id, post.Title, post.Content, post.CreatedAt));
    }

    public async Task<Result<IEnumerable<ForumAndPostsDTO>>> GetMyPostsAsync(UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var forumsAndPosts = await _postsRepository.GetMyForumsAndPostsAsync(user);

        return Result<IEnumerable<ForumAndPostsDTO>>.Success(forumsAndPosts.Select(forumAndPosts => 
            new ForumAndPostsDTO(forumAndPosts.Forum.Id, 
            forumAndPosts.Forum.Name, 
            forumAndPosts.Forum.Description, 
            forumAndPosts.Forum.CreatedAt,
            forumAndPosts.Posts.Select(post => 
                new PostDTO(post.Id, post.Title, post.Content, post.CreatedAt)))));
    }

    public async Task<Result<IEnumerable<PostDTO>>> GetPostsAsync()
    {
        var posts = await _postsRepository.GetAllAsync();

        return Result<IEnumerable<PostDTO>>.Success(posts.Select(post => new PostDTO(post.Id, post.Title, post.Content, post.CreatedAt)));
    }

    public async Task<Result<IEnumerable<PostDTO>>> GetPostsByForumIdAsync(Guid forumId)
    {
        var posts = await _postsRepository.GetPostsByForumIdAsync(forumId);

        return Result<IEnumerable<PostDTO>>.Success(posts.Select(post => new PostDTO(post.Id, post.Title, post.Content, post.CreatedAt)));
    }
}