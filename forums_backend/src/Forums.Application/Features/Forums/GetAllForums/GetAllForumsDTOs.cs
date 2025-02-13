namespace forums_backend.src.Forums.Application.Features.Forums.GetAllForums;

public record ForumDTO(Guid Id, string Name, string Description, int NumberOfPosts);