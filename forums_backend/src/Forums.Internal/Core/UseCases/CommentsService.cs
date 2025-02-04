using forums_backend.src.Forums.Internal.API.DTOs.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class CommentsService : ICommentsService
{
    private readonly ICommentsRepository _commentsRepository;

    public CommentsService(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }
    public async Task<CommentDTO> CreateCommentAsync(CreateCommentDTO createCommentDTO, UserDTO userDTO)
    {
        Comment comment = new(createCommentDTO.Content);
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        await _commentsRepository.AddAsync(comment, createCommentDTO.PostId, user);

        return new CommentDTO(comment.Id, comment.Content, comment.CreatedAt);
    }

    public async Task<PostAndCommentsDTO> GetPostAndItsCommentsAsync(Guid postId)
    {
        var postAndComments = await _commentsRepository.GetPostAndItsCommentsAsync(postId);

        var postDto = new PostDTO(postAndComments.Post.Id, postAndComments.Post.Title, postAndComments.Post.Content, postAndComments.Post.CreatedAt);

        var commentsDto = postAndComments.Comments.Select(c => new CommentDTO(c.Id, c.Content, c.CreatedAt));

        return new PostAndCommentsDTO(postDto, commentsDto);
    }

    public async Task<IEnumerable<CommentDTO>> GetMyCommentsAsync(UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var comments = await _commentsRepository.GetMyCommentsAsync(user);

        return comments.Select(c => new CommentDTO(c.Id, c.Content, c.CreatedAt));
    }
}