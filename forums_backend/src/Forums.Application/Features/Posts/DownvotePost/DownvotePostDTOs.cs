namespace forums_backend.src.Forums.Application.Features.Posts.DownvotePost;

public record DownvotePostDTO(Guid PostId, bool IsDownvoted);