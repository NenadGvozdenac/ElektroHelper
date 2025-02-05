namespace forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;

public record DownvotePostDTO(Guid PostId, int Upvotes, bool IsUpvoted);