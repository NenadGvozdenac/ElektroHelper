namespace forums_backend.src.Forums.Application.Features.Posts.SyncAllPosts;

public record ForumDTO(Guid Id, string Name);
public record PostDTO(Guid Id, string Title, string Content, ForumDTO Forum);