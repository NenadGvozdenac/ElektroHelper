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
        CreateMap<Comment, CommentDTO>();

        CreateMap<Post, PostDTO>();

        CreateMap<User, UserDTO>();

        CreateMap<User, UserResponseDTO>();

        CreateMap<Comment, CommentWithUserAndUpvotesDTO>();

        CreateMap<Forum, ForumDTO>();

        CreateMap<PostAndCommentsWithUpvotesAndDownvotes, CommentWithUserAndUpvotesDTO>();

        CreateMap<CommentWithUpvotesAndDownvotes, CommentWithUserAndUpvotesDTO>()
            .ForMember(dest => dest.Upvotes, opt => opt.MapFrom(src => src.Upvotes.Select(upvote =>
                new UserDTO(upvote.User.Id, upvote.User.Email, upvote.User.Role, upvote.User.Username)).ToList()))
            .ForMember(dest => dest.Downvotes, opt => opt.MapFrom(src => src.Downvotes.Select(downvote =>
                new UserDTO(downvote.User.Id, downvote.User.Email, downvote.User.Role, downvote.User.Username)).ToList()))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src =>
                new UserDTO(src.Creator.Id, src.Creator.Email, src.Creator.Role, src.Creator.Username)))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Comment.Id))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Comment.Content))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Comment.CreatedAt))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.Comment.IsDeleted));

        CreateMap<PostAndCommentsWithUpvotesAndDownvotes, PostAndCommentsDTO>()
            .ForMember(dest => dest.Post, opt => opt.MapFrom(src => src.Post))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Select(c => new CommentWithUserAndUpvotesDTO(
                c.Comment.Id,
                c.Comment.Content,
                c.Comment.CreatedAt,
                new UserDTO(c.Creator.Id, c.Creator.Email, c.Creator.Role, c.Creator.Username),
                c.Upvotes.Select(upvote => new UserDTO(upvote.User.Id, upvote.User.Email, upvote.User.Role, upvote.User.Username)).ToList(),
                c.Downvotes.Select(downvote => new UserDTO(downvote.User.Id, downvote.User.Email, downvote.User.Role, downvote.User.Username)).ToList()
            ))));
    }
}