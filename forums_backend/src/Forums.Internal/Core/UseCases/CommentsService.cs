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
            Id = Guid.NewGuid(),
            Content = createCommentDTO.Content,
            CreatedAt = DateTime.UtcNow,
        };

        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        PostVoting? post = await _postsRepository.GetByIdAsync(createCommentDTO.PostId, userDTO);

        if (post == null)
        {
            return Result<CommentDTO>.Failure("Post not found").WithCode(404);
        }

        await _commentsRepository.AddAsync(comment, createCommentDTO.PostId, user);

        CommentDTO commentDTO = _mapper.Map<CommentDTO>(comment);

        return Result<CommentDTO>.Success(commentDTO);
    }
    
    public async Task<Result<IEnumerable<CommentAndVotingDTO>>> GetCommentsForPost(Guid postId, UserDTO userDTO) {
        var comments = await _commentsRepository.GetCommentsForPost(postId, userDTO);

        var commentDTOs = _mapper.Map<IEnumerable<CommentAndVotingDTO>>(comments);

        return Result<IEnumerable<CommentAndVotingDTO>>.Success(commentDTOs);
    }

    public async Task<Result<IEnumerable<CommentDTO>>> GetMyCommentsAsync(UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var comments = await _commentsRepository.GetMyCommentsAsync(user);

        var commentDTOs = _mapper.Map<IEnumerable<CommentDTO>>(comments);

        return Result<IEnumerable<CommentDTO>>.Success(commentDTOs);
    }
}