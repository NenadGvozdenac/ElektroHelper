namespace forums_backend.src.Forums.Internal.API.DTOs.Posts;

public record CreatePostDTO(string Title, string Content, Guid ForumId);