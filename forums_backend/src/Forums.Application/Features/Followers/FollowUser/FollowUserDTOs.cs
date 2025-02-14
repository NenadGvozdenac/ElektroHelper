namespace forums_backend.src.Forums.Application.Features.Followers.FollowUser;

public record FollowUserDTO(string UserId, string FollowerId, bool Followed);