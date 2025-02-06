namespace forums_backend.src.Forums.Internal.API.DTOs.Comments;

public class CreateCommentDTO
{
    public string Content { get; set; } = string.Empty;
    public Guid PostId { get; set; }

    public CreateCommentDTO() { }

    public CreateCommentDTO(string content, Guid postId)
    {
        Content = content;
        PostId = postId;
    }
}