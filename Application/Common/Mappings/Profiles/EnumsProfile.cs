using AutoMapper;
using Domain.Enums;

namespace Application.Common.Mappings.Profiles;

public sealed class EnumsProfile : Profile
{
    public EnumsProfile()
    {
        CreateMap<string, RoleType>();
    }
}
