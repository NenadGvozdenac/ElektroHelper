using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface IUserService {
    public Task<Result<UserResponseDTO>> CreateUserAsync(UserDTO user);
}