using Application.Common.Models.DTOs;
using AutoMapper;
using Domain.Entities.Companies;

namespace Application.Common.Mappings.Profiles;

public sealed class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.Id,
            conf => conf.MapFrom(src => src.Id.Value));
    }
}
