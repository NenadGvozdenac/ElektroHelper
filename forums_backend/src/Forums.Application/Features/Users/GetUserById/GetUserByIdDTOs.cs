namespace forums_backend.src.Forums.Application.Features.Users.GetUserById;

public record UserByIdDTO(string Id, string Username, string Email, string Role, bool IsBanned, string ReasonForBan, bool IsDeleted);