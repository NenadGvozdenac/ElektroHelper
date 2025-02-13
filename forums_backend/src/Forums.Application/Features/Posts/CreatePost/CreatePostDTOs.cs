namespace forums_backend.src.Forums.Application.Features.Posts.CreatePost;

public record CreatePostDTO(string Title, string Content, Guid ForumId);
public record CreatedPostDTO(Guid PostId, string Title, string Content, DateTime CreatedAt);