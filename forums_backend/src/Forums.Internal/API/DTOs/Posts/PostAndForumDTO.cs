namespace forums_backend.src.Forums.Internal.API.DTOs.Posts;

public record ForumAndPostsDTO(Guid ForumId, string ForumTitle, string ForumDescription, DateTime CreatedAt, IEnumerable<PostDTO> Posts);