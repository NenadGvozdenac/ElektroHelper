using AutoMapper;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.Internal.Core.Domain;
using Neo4j.Driver;

namespace forums_backend.src.Forums.Internal.Infrastructure.Mappings;

public class Neo4jMapperProfile : Profile
{
    public Neo4jMapperProfile()
    {
        CreateMap<INode, Post>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src["id"].As<string>())))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src["title"].As<string>()))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src["content"].As<string>()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src["createdAt"].As<string>().FromNeo4jDateTime()))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => bool.Parse(src["isDeleted"].As<string>())))
            .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => bool.Parse(src["isLocked"].As<string>())));

        CreateMap<INode, Forum>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src["id"].As<string>())))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["name"].As<string>()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src["description"].As<string>()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src["createdAt"].As<string>().FromNeo4jDateTime()))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => bool.Parse(src["isDeleted"].As<string>())))
            .ForMember(dest => dest.IsQuarantined, opt => opt.MapFrom(src => bool.Parse(src["isQuarantined"].As<string>())));

        CreateMap<INode, Comment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src["id"].As<string>())))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src["content"].As<string>()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src["createdAt"].As<string>().FromNeo4jDateTime()))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => bool.Parse(src["isDeleted"].As<string>())));
    }
}