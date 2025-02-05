using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class CommentsService : ICommentsService
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IPostsRepository _postsRepository;

    public CommentsService(ICommentsRepository commentsRepository, IPostsRepository postsRepository)
    {
        _commentsRepository = commentsRepository;
        _postsRepository = postsRepository;
    }
    public async Task<Result<CommentDTO>> CreateCommentAsync(CreateCommentDTO createCommentDTO, UserDTO userDTO)
    {
        Comment comment = new(createCommentDTO.Content);
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        Post? post = await _postsRepository.GetByIdAsync(createCommentDTO.PostId);

        if (post == null)
        {
            return Result<CommentDTO>.Failure("Post not found").WithCode(404);
        }

        await _commentsRepository.AddAsync(comment, createCommentDTO.PostId, user);

        return Result<CommentDTO>.Success(new CommentDTO(comment.Id, comment.Content, comment.CreatedAt));
    }

    public async Task<Result<PostAndCommentsDTO>> GetPostAndItsCommentsAsync(Guid postId)
    {
        var postAndComments = await _commentsRepository.GetPostAndItsCommentsAsync(postId);

        if(postAndComments == null)
        {
            return Result<PostAndCommentsDTO>.Failure("Post not found!").WithCode(404);
        }

        var postDto = new PostDTO(postAndComments.Post.Id, postAndComments.Post.Title, postAndComments.Post.Content, postAndComments.Post.CreatedAt);

        var commentsDto = postAndComments.Comments.Select(c => new CommentWithUserDTO(c.Comment.Id, c.Comment.Content, c.Comment.CreatedAt, new UserDTO(c.User.Id, c.User.Email, c.User.Role, c.User.Username)));

        return Result<PostAndCommentsDTO>.Success(new PostAndCommentsDTO(postDto, commentsDto));
    }

    public async Task<Result<IEnumerable<CommentDTO>>> GetMyCommentsAsync(UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var comments = await _commentsRepository.GetMyCommentsAsync(user);

        return Result<IEnumerable<CommentDTO>>.Success(comments.Select(c => new CommentDTO(c.Id, c.Content, c.CreatedAt)));
    }
}