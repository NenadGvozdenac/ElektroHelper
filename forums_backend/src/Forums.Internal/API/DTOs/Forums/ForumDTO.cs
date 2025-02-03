namespace forums_backend.src.Forums.Internal.API.DTOs.Forums;

public record ForumDTO(Guid Id, string Name, string Description, DateTime CreatedAt);