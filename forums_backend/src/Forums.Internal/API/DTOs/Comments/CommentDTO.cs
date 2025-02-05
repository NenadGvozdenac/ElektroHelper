using forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.DTOs.Comments;

public record CommentDTO(Guid Id, string Content, DateTime CreatedAt);

public record CommentWithUserDTO(Guid Id, string Content, DateTime CreatedAt, UserDTO User);

public record CommentWithUserAndUpvotesDTO(Guid Id, string Content, DateTime CreatedAt, UserDTO User, List<UserDTO> Upvotes, List<UserDTO> Downvotes) {
    public int UpvotesCount => Upvotes.Count;
    public int DownvotesCount => Downvotes.Count;
}