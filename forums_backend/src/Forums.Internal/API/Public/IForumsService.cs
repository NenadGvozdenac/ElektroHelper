using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface IForumsService {
    public Task<Result<ForumDTO>> CreateForumAsync(CreateForumDTO createCategoryDTO, UserDTO user);
    public Task<Result<IEnumerable<ForumDTO>>> GetForumsAsync();
}