namespace forums_backend.src.Forums.Application.Features.Posts.GetMyPosts;

public record ForumDTO(Guid Id, string Name);
public record PostDTO(Guid Id,
    string Title,
    string Content, 
    DateTime CreatedAt, 
    bool IsUpvoted, 
    bool IsDownvoted, 
    int Upvotes, 
    int Downvotes,
    int Comments,
    ForumDTO Forum);