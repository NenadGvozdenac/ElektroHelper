using AutoMapper;
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
    private readonly IMapper _mapper;

    public CommentsService(ICommentsRepository commentsRepository, IPostsRepository postsRepository, IMapper mapper)
    {
        _commentsRepository = commentsRepository;
        _postsRepository = postsRepository;
        _mapper = mapper;
    }
    public async Task<Result<CommentDTO>> CreateCommentAsync(CreateCommentDTO createCommentDTO, UserDTO userDTO)
    {
        var comment = new Comment{
            Content = createCommentDTO.Content,
        };

        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        Post? post = await _postsRepository.GetByIdAsync(createCommentDTO.PostId);

        if (post == null)
        {
            return Result<CommentDTO>.Failure("Post not found").WithCode(404);
        }

        await _commentsRepository.AddAsync(comment, createCommentDTO.PostId, user);

        CommentDTO commentDTO = _mapper.Map<CommentDTO>(comment);

        return Result<CommentDTO>.Success(commentDTO);
    }

    public async Task<Result<PostAndCommentsDTO>> GetPostAndItsCommentsAsync(Guid postId)
    {
        var postAndComments = await _commentsRepository.GetPostAndItsCommentsAsync(postId);

        if (postAndComments == null)
        {
            return Result<PostAndCommentsDTO>.Failure("Post not found!").WithCode(404);
        }

        PostDTO postDto = _mapper.Map<PostDTO>(postAndComments.Post);

        List<CommentWithUserAndUpvotesDTO> comments = postAndComments.Comments.Select(c => _mapper.Map<CommentWithUserAndUpvotesDTO>(c)).ToList();

        List<UserDTO> upvoters = postAndComments.Upvoters.Select(u => new UserDTO(u.User.Id, u.User.Email, u.User.Role, u.User.Username)).ToList();
        List<UserDTO> downvoters = postAndComments.Downvoters.Select(u => new UserDTO(u.User.Id, u.User.Email, u.User.Role, u.User.Username)).ToList();
        UserDTO originalPoster = new UserDTO(postAndComments.Creator.Id, postAndComments.Creator.Email, postAndComments.Creator.Role, postAndComments.Creator.Username);

        PostAndCommentsDTO postAndCommentsDto = new PostAndCommentsDTO(postDto, comments, upvoters, downvoters, originalPoster);
        return Result<PostAndCommentsDTO>.Success(postAndCommentsDto);
    }

    public async Task<Result<IEnumerable<CommentDTO>>> GetMyCommentsAsync(UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var comments = await _commentsRepository.GetMyCommentsAsync(user);

        var commentDTOs = _mapper.Map<IEnumerable<CommentDTO>>(comments);

        return Result<IEnumerable<CommentDTO>>.Success(commentDTOs);
    }
}