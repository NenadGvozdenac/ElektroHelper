namespace forums_backend.src.Forums.Application.Features.Comments.DownvoteComment;

public record DownvoteCommentDTO(Guid CommentId, bool IsDownvoted);