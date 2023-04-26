using Apllication.Common.Models.DTOs;
using AutoMapper;
using Domain.Entities.Platforms;

namespace Apllication.Common.Mappings.Profiles;

public sealed class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<Platform, PlatformDto>();
    }
}
