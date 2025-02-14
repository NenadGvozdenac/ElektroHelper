namespace forums_backend.src.Forums.Application.Features.Followers.UnfollowUser;

public record UnfollowUserDTO(string UserId, string FollowerId, bool Followed);