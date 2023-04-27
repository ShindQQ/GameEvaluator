using Apllication.Common.Models.DTOs;
using AutoMapper;
using Domain.Entities.Users;

namespace Apllication.Common.Mappings.Profiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Games,
            conf => conf.MapFrom(src => src.Games.Select(userGame => userGame.Game)))
            .ForMember(dest => dest.Id,
            conf => conf.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Company,
            conf => conf.MapFrom(src => src.Company!.Name))
            .ForMember(dest => dest.Roles,
            conf => conf.MapFrom(src => src.Roles.Select(role => role.Name)));
    }
}
