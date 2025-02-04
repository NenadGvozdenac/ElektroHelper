namespace forums_backend.src.Forums.Internal.API.DTOs.Comments;

public record CommentDTO(Guid Id, string Content, DateTime CreatedAt);