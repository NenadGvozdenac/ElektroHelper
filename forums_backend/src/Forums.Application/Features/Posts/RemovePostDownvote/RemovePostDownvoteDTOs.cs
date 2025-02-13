namespace forums_backend.src.Forums.Application.Features.Posts.RemovePostDownvote;

public record PostDownvoteDTO(Guid PostId, bool IsDownvoted);