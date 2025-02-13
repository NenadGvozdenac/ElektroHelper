namespace forums_backend.src.Forums.Application.Features.Posts.GetPostsByForumIdPaged;

public record AuthorDTO(string Id, string Username, string Email);
public record PostDTO(Guid Id,
    string Title,
    string Content,
    DateTime CreatedAt,
    bool IsUpvoted,
    bool IsDownvoted,
    int Upvotes,
    int Downvotes,
    int Comments,
    AuthorDTO Author);