using Application.Common.Models.DTOs;
using AutoMapper;
using Domain.Entities.Comments;

namespace Application.Common.Mappings.Profiles;

public sealed class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.Id,
            conf => conf.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.GameId,
            conf => conf.MapFrom(src => src.GameId.Value))
            .ForMember(dest => dest.ParentCommentId,
            conf => conf.MapFrom(src => src.ParentCommentId!.Value))
            .ForMember(dest => dest.ChildrenComments,
            conf => conf.MapFrom(src => src.ChildrenComments))
            .ForMember(dest => dest.LeftBy,
            conf => conf.MapFrom(src => src.User.Name))
            .MaxDepth(4);

        //CreateMap<Comment, CommentDto>()
        //    .ForMember(dest => dest,
        //    conf => conf.MapFrom<CommentResolver>());
    }
}
