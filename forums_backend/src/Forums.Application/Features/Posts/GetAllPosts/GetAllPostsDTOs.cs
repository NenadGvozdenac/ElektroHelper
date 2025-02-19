namespace forums_backend.src.Forums.Application.Features.Posts.GetAllPosts;

public record AuthorDTO(string Id, string Username, string Email);
public record ForumDTO(Guid Id, string Name);
public record PostDTO(Guid Id,
    string Title,
    string Content, 
    DateTime CreatedAt, 
    bool IsUpvoted, 
    bool IsDownvoted, 
    bool IsDeleted,
    int Upvotes, 
    int Downvotes,
    int Comments,
    AuthorDTO Author,
    ForumDTO Forum);
