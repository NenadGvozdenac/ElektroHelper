namespace forums_backend.src.Forums.Application.Features.Comments.GetCommentsForPost;

public record AuthorDTO(
    string Id,
    string Username,
    string Email,
    string Role
);

public record CommentDTO(
    Guid Id,
    string Content,
    AuthorDTO Author,
    DateTime CreatedAt,
    int Upvotes,
    int Downvotes,
    bool IsUpvoted,
    bool IsDownvoted,
    bool IsDeleted
);