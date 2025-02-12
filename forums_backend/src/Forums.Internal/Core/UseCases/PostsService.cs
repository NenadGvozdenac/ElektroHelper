using AutoMapper;
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
    private readonly IMapper _mapper;

    public PostsService(IPostsRepository postsRepository, IForumsRepository forumsRepository, IMapper mapper)
    {
        _postsRepository = postsRepository;
        _forumsRepository = forumsRepository;
        _mapper = mapper;
    }

    public async Task<Result<PostDTO>> CreatePostAsync(CreatePostDTO createPostDTO, UserDTO userDTO)
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Title = createPostDTO.Title,
            Content = createPostDTO.Content,
            CreatedAt = DateTime.UtcNow
        };

        Forum? forum = await _forumsRepository.GetByIdAsync(createPostDTO.ForumId);

        if (forum == null)
        {
            return Result<PostDTO>.Failure("Forum not found").WithCode(404);
        }

        Post? addedPost = await _postsRepository.AddAsync(post, createPostDTO.ForumId, userDTO);

        if (addedPost == null)
        {
            return Result<PostDTO>.Failure("Failed to create post");
        }

        PostDTO postDTO = _mapper.Map<PostDTO>(addedPost);

        return Result<PostDTO>.Success(postDTO);
    }

    public async Task<Result<IEnumerable<ForumAndPostsDTO>>> GetMyPostsAsync(UserDTO userDTO)
    {
        var forumsAndPosts = await _postsRepository.GetMyForumsAndPostsAsync(userDTO);

        var forumAndPostsDTOs = forumsAndPosts.Select(forumAndPosts => new ForumAndPostsDTO(
            forumAndPosts.Forum.Id,
            forumAndPosts.Forum.Name,
            forumAndPosts.Forum.Description,
            forumAndPosts.Forum.CreatedAt,
            forumAndPosts.Posts.Select(post => _mapper.Map<PostDTO>(post))));

        return Result<IEnumerable<ForumAndPostsDTO>>.Success(forumAndPostsDTOs);
    }

    public async Task<Result<PostDTO>> GetPostByIdAsync(Guid postId, UserDTO userDTO)
    {
        var post = await _postsRepository.GetByIdAsync(postId, userDTO);

        if (post == null)
        {
            return Result<PostDTO>.Failure("Post not found").WithCode(404);
        }

        var postDTO = _mapper.Map<PostDTO>(post);

        return Result<PostDTO>.Success(postDTO);
    }

    public async Task<Result<IEnumerable<PostDTO>>> GetPostsAsync()
    {
        var posts = await _postsRepository.GetAllAsync();

        var postDTOs = _mapper.Map<IEnumerable<PostDTO>>(posts);

        return Result<IEnumerable<PostDTO>>.Success(postDTOs);
    }

    public async Task<Result<IEnumerable<PostDTO>>> GetPostsAsync(int page, int pageSize, UserDTO userDTO)
    {
        var posts = await _postsRepository.GetPagedAsync(page, pageSize, userDTO);

        var postDTOs = _mapper.Map<IEnumerable<PostDTO>>(posts);

        return Result<IEnumerable<PostDTO>>.Success(postDTOs);
    }

    public async Task<Result<IEnumerable<PostDTO>>> GetPostsByForumIdAsync(Guid forumId, UserDTO userDTO)
    {
        var posts = await _postsRepository.GetPostsByForumIdAsync(forumId, userDTO);

        var postDTOs = _mapper.Map<IEnumerable<PostDTO>>(posts);

        return Result<IEnumerable<PostDTO>>.Success(postDTOs);
    }

    public async Task<Result<IEnumerable<PostDTO>>> GetPostsByForumIdPagedAsync(int page, int pageSize, Guid forumId, UserDTO userDTO)
    {
        var posts = await _postsRepository.GetPostsByForumIdPagedAsync(page, pageSize, forumId, userDTO);

        var postDTOs = _mapper.Map<IEnumerable<PostDTO>>(posts);

        return Result<IEnumerable<PostDTO>>.Success(postDTOs);
    }
}