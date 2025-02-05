using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.DTOs.Comments;

public record CommentDTO(Guid Id, string Content, DateTime CreatedAt);

public record CommentWithUserDTO(Guid Id, string Content, DateTime CreatedAt, UserDTO User);