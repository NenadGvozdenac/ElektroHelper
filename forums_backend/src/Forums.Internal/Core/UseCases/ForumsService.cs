using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class ForumsService : IForumsService
{
    private readonly IForumsRepository _forumsRepository;

    public ForumsService(IForumsRepository forumsRepository)
    {
        _forumsRepository = forumsRepository;
    }

    public async Task<Result<ForumDTO>> CreateForumAsync(CreateForumDTO createCategoryDTO, UserDTO userDTO)
    {
        Forum forum = new(createCategoryDTO.Name, createCategoryDTO.Description);
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        await _forumsRepository.AddAsync(forum, user);

        return Result<ForumDTO>.Success(new ForumDTO(forum.Id, forum.Name, forum.Description, forum.CreatedAt));
    }

    public async Task<Result<IEnumerable<ForumDTO>>> GetForumsAsync()
    {
        var forums = await _forumsRepository.GetAllAsync();

        return Result<IEnumerable<ForumDTO>>.Success(forums.Select(forum => new ForumDTO(forum.Id, forum.Name, forum.Description, forum.CreatedAt)));
    }
}