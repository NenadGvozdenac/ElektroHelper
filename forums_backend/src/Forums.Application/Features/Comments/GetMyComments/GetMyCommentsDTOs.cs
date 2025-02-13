namespace forums_backend.src.Forums.Application.Features.Comments.GetMyComments;

public record CommentDTO(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    int Upvotes,
    int Downvotes,
    bool IsUpvoted,
    bool IsDownvoted,
    bool IsDeleted
);