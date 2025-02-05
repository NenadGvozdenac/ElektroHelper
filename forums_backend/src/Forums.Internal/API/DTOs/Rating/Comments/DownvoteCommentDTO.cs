namespace forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;

public record DownvoteCommentDTO(Guid CommentId, bool IsDownvoted);