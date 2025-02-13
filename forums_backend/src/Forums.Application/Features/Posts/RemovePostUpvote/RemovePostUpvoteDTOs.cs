namespace forums_backend.src.Forums.Application.Features.Posts.RemovePostUpvote;

public record UpvotePostDTO(Guid PostId, bool IsUpvoted);