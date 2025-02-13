namespace forums_backend.src.Forums.Application.Features.Comments.RemoveCommentDownvote;

public record CommentDownvoteDTO(Guid CommentId, bool IsDownvoted);