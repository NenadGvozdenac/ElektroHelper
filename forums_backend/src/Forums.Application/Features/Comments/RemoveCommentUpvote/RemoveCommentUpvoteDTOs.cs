namespace forums_backend.src.Forums.Application.Features.Comments.RemoveCommentUpvote;

public record CommentUpvoteDTO(Guid CommentId, bool IsUpvoted);