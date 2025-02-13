namespace forums_backend.src.Forums.API.DTOs;

public record CreateCommentDTO(string Content, Guid PostId);