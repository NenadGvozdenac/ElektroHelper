namespace forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;

public class DownvoteCommentDTO
{
    public Guid CommentId { get; set; }
    public bool IsDownvoted { get; set; }

    // Parameterless constructor
    public DownvoteCommentDTO() { }

    // Constructor for convenience
    public DownvoteCommentDTO(Guid commentId, bool isDownvoted)
    {
        CommentId = commentId;
        IsDownvoted = isDownvoted;
    }
}