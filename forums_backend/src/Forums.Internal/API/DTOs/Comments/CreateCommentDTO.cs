namespace forums_backend.src.Forums.Internal.API.DTOs.Comments;

public record CreateCommentDTO(string Content, Guid PostId);