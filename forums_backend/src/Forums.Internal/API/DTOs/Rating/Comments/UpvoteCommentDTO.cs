namespace forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;

public record UpvoteCommentDTO(Guid CommentId, int Upvotes, bool IsUpvoted);