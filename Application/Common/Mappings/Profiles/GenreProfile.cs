using Application.Common.Models.DTOs;
using AutoMapper;
using Domain.Entities.Genres;

namespace Application.Common.Mappings.Profiles;

public sealed class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreDto>()
            .ForMember(dest => dest.Id,
            conf => conf.MapFrom(src => src.Id.Value));
    }
}
