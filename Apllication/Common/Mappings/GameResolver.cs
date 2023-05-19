using Application.Common.Models.DTOs;
using Domain.Entities.Games;

namespace Application.Common.Mappings;

public static class GameResolver
{
    public static IQueryable<GameDto>? IQueryableMapGamesToGameDtos(IQueryable<Game> queryable)
    {
        return queryable.Select(game => new GameDto
        {
            UsersAmmount = game.GameUsers.Count,
            AverageRating = game.GameUsers.Average(entity => entity.Rating),
            Id = game.Id.Value,
            Genres = game.Genres.Select(genre => genre.Name).ToList(),
            Platforms = game.Platforms.Select(platform => platform.Name).ToList(),
            CompaniesNames = game.Companies.Select(company => company.Name).ToList(),
            Description = game.Description,
            Name = game.Name,
            Comments = game.Comments.Select(comment => CommentResolver.MapCommentToCommentDto(comment)!).ToList()
        });
    }

    public static IEnumerable<GameDto>? IEnumerableMapGamesToGameDtos(IEnumerable<Game> queryable)
    {
        return queryable.Select(game => new GameDto
        {
            UsersAmmount = game.GameUsers.Count,
            AverageRating = game.GameUsers.Average(entity => entity.Rating),
            Id = game.Id.Value,
            Genres = game.Genres.Select(genre => genre.Name).ToList(),
            Platforms = game.Platforms.Select(platform => platform.Name).ToList(),
            CompaniesNames = game.Companies.Select(company => company.Name).ToList(),
            Description = game.Description,
            Name = game.Name,
            Comments = game.Comments.Select(comment => CommentResolver.MapCommentToCommentDto(comment)!).ToList()
        });
    }
}
