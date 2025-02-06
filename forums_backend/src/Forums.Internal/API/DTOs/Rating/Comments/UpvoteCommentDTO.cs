namespace forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;

public class UpvoteCommentDTO
{
    public Guid CommentId { get; set; }
    public bool IsUpvoted { get; set; }

    public UpvoteCommentDTO() { }

    public UpvoteCommentDTO(Guid commentId, bool isUpvoted)
    {
        CommentId = commentId;
        IsUpvoted = isUpvoted;
    }
}