namespace forums_backend.src.Forums.Application.Features.Forums.GetForumById;

public record ForumDTO(Guid Id, string Name, string Description, int NumberOfPosts, bool IsQuarantined);