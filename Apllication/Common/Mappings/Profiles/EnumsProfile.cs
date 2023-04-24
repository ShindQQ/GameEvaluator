using AutoMapper;
using Domain.Enums;

namespace Apllication.Common.Mappings.Profiles;

public sealed class EnumsProfile : Profile
{
    public EnumsProfile()
    {
        CreateMap<string, RoleType>();
        CreateMap<string, GenreType>();
        CreateMap<string, PlatformType>();
    }
}
