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

    public string? RefreshToken { get; private set; }

    public DateTime? RefreshTokenExpiryTime { get; private set; }

    public HashSet<UserGame> Games { get; private set; } = new();

    public HashSet<Role> Roles { get; private set; } = new();

    public CompanyId? CompanyId { get; private set; }

    public Company? Company { get; private set; }

    public Ban? BanState { get; private set; }

    public static User Create(string name,
        string email,
        string password)
    {
        var passwordHash = PasswordHasher.Hash(password);

        var user = new User
        {
            Id = new UserId(Guid.NewGuid()),
            Name = name,
            Email = email,
            Password = passwordHash,
        };

        user.AddRole(RoleType.User);

        return user;
    }

    public void Ban(DateTime banTo)
    {
        BanState = ValueObjects.Ban.Create(banTo);
    }

    public void UnBan()
    {
        BanState = null;
    }

    public void SetRefreshToken(string refreshToken, DateTime expireTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expireTime;
    }

    public void Update(string? name, string? email)
    {
        if (name is not null)
            Name = name;

        if (email is not null)
            Email = email;
    }

    public bool VerifyPassword(string password)
        => PasswordHasher.Verify(password, Password);

    public bool AddRole(RoleType roleType)
        => Roles.Add(Role.Create(roleType));

    public bool RemoveRole(RoleType roleType)
        => Roles.Remove(Role.Create(roleType));

    public bool AddGame(Game game)
        => Games.Add(UserGame.Create(this, game));

    public bool ChangeFavoriteState(GameId gameId)
    {
        var userGame = Games.FirstOrDefault(game => game.GameId == gameId);

        if (userGame is null)
            return false;

        userGame.ChangeFavoriteState();

        return true;
    }

    public bool SetRatingToTheGame(GameId gameId, int rating)
    {
        var userGame = Games.FirstOrDefault(game => game.GameId == gameId);

        if (userGame is null)
            return false;

        userGame.SetRating(rating);

        return true;
    }
}
