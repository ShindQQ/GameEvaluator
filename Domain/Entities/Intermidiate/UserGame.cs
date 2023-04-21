using Domain.Entities.Games;
using Domain.Entities.Users;

namespace Domain.Entities.Intermidiate;

public sealed class UserGame
{
    public GameId GameId { get; private set; } = null!;

    public Game Game { get; private set; } = null!;

    public int Rating { get; private set; }

    public bool IsFavorite { get; private set; } = false;

    public UserId UserId { get; private set; } = null!;

    public User User { get; private set; } = null!;

    public UserGame(User user, Game game)
    {
        UserId = user.Id;
        User = user;
        GameId = game.Id;
        Game = game;
    }

    public void SetFavorite()
        => IsFavorite = true;

    public void RemoveFavorite()
        => IsFavorite = false;

    public void SetRating(int rating)
        => Rating = rating;
}
