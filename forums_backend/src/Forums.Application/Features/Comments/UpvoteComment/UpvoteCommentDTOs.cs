namespace forums_backend.src.Forums.Application.Features.Comments.UpvoteComment;

public record UpvoteCommentDTO(Guid CommentId, bool IsUpvoted);