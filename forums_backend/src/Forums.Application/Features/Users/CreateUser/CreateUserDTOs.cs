namespace forums_backend.src.Forums.Application.Features.Users.CreateUser;

public record CreateUserDTO(string Id, string Username, string Email, string Role);
public record CreatedUserDTO(string Id, string Username, string Email, string Role);