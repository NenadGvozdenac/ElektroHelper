using AutoMapper;
using forums_backend.src.Forums.Internal.API.DTOs.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.Core.Domain;

namespace forums_backend.src.Forums.Internal.Infrastructure.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Basic mappings
        CreateMap<Comment, CommentDTO>()
            .ForMember(dest => dest.NumberOfUpvotes, opt => opt.MapFrom(src => src.NumberOfUpvotes))
            .ForMember(dest => dest.NumberOfDownvotes, opt => opt.MapFrom(src => src.NumberOfDownvotes));
            
        CreateMap<Post, PostDTO>();
        
        CreateMap<PostVoting, PostDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Post.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Post.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Post.Content))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Post.CreatedAt))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.Post.IsDeleted))
            .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.Post.IsLocked))
            .ForMember(dest => dest.NumberOfUpvotes, opt => opt.MapFrom(src => src.Post.NumberOfUpvotes))
            .ForMember(dest => dest.NumberOfDownvotes, opt => opt.MapFrom(src => src.Post.NumberOfDownvotes))
            .ForMember(dest => dest.NumberOfComments, opt => opt.MapFrom(src => src.Post.NumberOfComments))
            .ForMember(dest => dest.IsUpvoted, opt => opt.MapFrom(src => src.IsUpvoted))
            .ForMember(dest => dest.IsDownvoted, opt => opt.MapFrom(src => src.IsDownvoted))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => new UserDTO(src.Author.Id, src.Author.Email, src.Author.Role, src.Author.Username)))
            .ForMember(dest => dest.Forum, opt => opt.MapFrom(src => new ForumDTO {
                Id = src.Forum.Id,
                Name = src.Forum.Name,
                Description = src.Forum.Description
            }));
            
        CreateMap<User, UserDTO>();
        CreateMap<User, UserResponseDTO>();
        CreateMap<Forum, ForumDTO>();

        CreateMap<CommentAndVoting, CommentAndVotingDTO>()
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.IsUpvoted, opt => opt.MapFrom(src => src.IsUpvoted))
            .ForMember(dest => dest.IsDownvoted, opt => opt.MapFrom(src => src.IsDownvoted));

        // Reusable user DTO mapping
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));

        // Reusable CommentWithUserAndUpvotesDTO mapping
        CreateMap<CommentWithUpvotesAndDownvotes, CommentWithUserAndUpvotesDTO>()
            .ForMember(dest => dest.Upvotes, opt => opt.MapFrom(src => MapUsersToUserDTO(src.Upvotes.Select(upvote => upvote.User))))
            .ForMember(dest => dest.Downvotes, opt => opt.MapFrom(src => MapUsersToUserDTO(src.Downvotes.Select(downvote => downvote.User))))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => new UserDTO(src.Creator.Id, src.Creator.Email, src.Creator.Role, src.Creator.Username)))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Comment.Id))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Comment.Content))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Comment.CreatedAt))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.Comment.IsDeleted));

        // Mapping for PostAndCommentsWithUpvotesAndDownvotes to PostAndCommentsDTO
        CreateMap<PostAndCommentsWithUpvotesAndDownvotes, PostAndCommentsDTO>()
            .ForMember(dest => dest.Post, opt => opt.MapFrom(src => src.Post))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => MapCommentsWithUserAndUpvotes(src.Comments)))
            .ForMember(dest => dest.Upvotes, opt => opt.MapFrom(src => MapUsersToUserDTO(src.Upvoters.Select(upvote => upvote.User))))
            .ForMember(dest => dest.Downvotes, opt => opt.MapFrom(src => MapUsersToUserDTO(src.Downvoters.Select(downvote => downvote.User))))
            .ForMember(dest => dest.OriginalPoster, opt => opt.MapFrom(src => new UserDTO(src.Creator.Id, src.Creator.Email, src.Creator.Role, src.Creator.Username)));
    }

    private static List<UserDTO> MapUsersToUserDTO(IEnumerable<User> users)
    {
        return users.Select(u => new UserDTO(u.Id, u.Email, u.Role, u.Username)).ToList();
    }

    private static List<CommentWithUserAndUpvotesDTO> MapCommentsWithUserAndUpvotes(IEnumerable<CommentWithUpvotesAndDownvotes> comments)
    {
        return comments.Select(c => new CommentWithUserAndUpvotesDTO(
            c.Comment.Id,
            c.Comment.Content,
            c.Comment.CreatedAt,
            new UserDTO(c.Creator.Id, c.Creator.Email, c.Creator.Role, c.Creator.Username),
            MapUsersToUserDTO(c.Upvotes.Select(upvote => upvote.User)),
            MapUsersToUserDTO(c.Downvotes.Select(downvote => downvote.User))
        )).ToList();
    }
}