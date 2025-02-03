using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class ForumsService(IForumsRepository forumsRepository) : IForumsService
{
    private readonly IForumsRepository _forumsRepository = forumsRepository;

    public Task CreateForumAsync(CreateForumDTO createCategoryDTO)
    {
        Forum forum = new(createCategoryDTO.Name, createCategoryDTO.Description);

        return _forumsRepository.AddAsync(forum);
    }

    public async Task<IEnumerable<ForumDTO>> GetForumsAsync()
    {
        var forums = await _forumsRepository.GetAllAsync();

        return forums.Select(forum => new ForumDTO(forum.Id, forum.Name, forum.Description));
    }
}