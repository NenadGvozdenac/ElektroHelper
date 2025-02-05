using forums_backend.src.Forums.Internal.API.DTOs.Posts;

namespace forums_backend.src.Forums.Internal.API.DTOs.Comments;

public record PostAndCommentsDTO(PostDTO Post, IEnumerable<CommentWithUserDTO> Comments);