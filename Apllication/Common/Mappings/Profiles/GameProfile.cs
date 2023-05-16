using Application.Common.Models.DTOs;
using AutoMapper;
using Domain.Entities.Games;

namespace Application.Common.Mappings.Profiles;

public sealed class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<Game, GameDto>()
            .ForMember(dest => dest.UsersAmmount,
            conf => conf.MapFrom(src => src.GameUsers.Count))
            .ForMember(dest => dest.AverageRating,
            conf => conf.MapFrom(src => src.GameUsers.Average(entity => entity.Rating)))
            .ForMember(dest => dest.Id,
            conf => conf.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Genres,
            conf => conf.MapFrom(src => src.Genres.Select(genre => genre.Name)))
            .ForMember(dest => dest.Platforms,
            conf => conf.MapFrom(src => src.Platforms.Select(platform => platform.Name)))
            .ForMember(dest => dest.CompaniesNames,
            conf => conf.MapFrom(src => src.Companies.Select(company => company.Name)))
            .ForMember(dest => dest.Comments,
            conf => conf.MapFrom(src => src.Comments));
    }
}
