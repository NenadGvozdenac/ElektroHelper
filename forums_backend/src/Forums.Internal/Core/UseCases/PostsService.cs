using forums_backend.src.Forums.Internal.API.DTOs.Posts;
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

    public async Task<PostDTO> CreatePostAsync(CreatePostDTO createPostDTO)
    {
        Post post = new(createPostDTO.Title, createPostDTO.Content);

        await _postsRepository.AddAsync(post, createPostDTO.ForumId);

        return new PostDTO(post.Id, post.Title, post.Content);
    }

    public async Task<IEnumerable<PostDTO>> GetPostsAsync()
    {
        var posts = await _postsRepository.GetAllAsync();

        return posts.Select(post => new PostDTO(post.Id, post.Title, post.Content));
    }

    public async Task<IEnumerable<PostDTO>> GetPostsByForumIdAsync(Guid forumId)
    {
        var posts = await _postsRepository.GetPostsByForumIdAsync(forumId);

        return posts.Select(post => new PostDTO(post.Id, post.Title, post.Content));
    }
}