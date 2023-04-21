using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Intermidiate;
using Domain.Enums;
using Domain.Helpers;
using Domain.ValueObjects;

namespace Domain.Entities.Users;

public sealed class User
{
    public UserId Id { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string Password { get; private set; } = string.Empty;

    public HashSet<UserGame> Games { get; private set; } = new();

    public HashSet<Role> Roles { get; private set; } = new();

    public CompanyId? CompanyId { get; private set; }

    public Company? Company { get; private set; }

    public User(string name,
        string email,
        string password)
    {
        var passwordHash = PasswordHasher.Hash(password);

        Id = new UserId(Guid.NewGuid());
        Name = name;
        Email = email;
        Password = passwordHash;
        AddRole(RoleType.User);
    }

    public void Update(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public static bool VerifyPassword(string password, string passwordHash)
        => PasswordHasher.Verify(password, passwordHash);

    public bool AddRole(RoleType roleType)
        => Roles.Add(new Role(roleType));

    public bool RemoveRole(RoleType roleType)
        => Roles.Remove(new Role(roleType));

    public bool AddGame(Game game)
        => Games.Add(new UserGame(this, game));

    public bool SetGameAsFavorite(GameId gameId)
    {
        var userGame = Games.FirstOrDefault(game => game.GameId == gameId);

        if (userGame == null)
            return false;

        userGame.SetFavorite();

        return true;
    }

    public bool ResetFavoriteGame(GameId gameId)
    {
        var userGame = Games.FirstOrDefault(game => game.GameId == gameId);

        if (userGame == null)
            return false;

        userGame.RemoveFavorite();

        return true;
    }

    public bool SetRatingToTheGame(GameId gameId, int rating)
    {
        var userGame = Games.FirstOrDefault(game => game.GameId == gameId);

        if (userGame == null)
            return false;

        userGame.SetRating(rating);

        return true;
    }
}
