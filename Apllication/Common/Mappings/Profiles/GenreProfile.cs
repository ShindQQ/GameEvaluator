using Apllication.Common.Models.DTOs;
using AutoMapper;
using Domain.Entities.Genres;

namespace Apllication.Common.Mappings.Profiles;

public sealed class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreDto>();
    }
}
