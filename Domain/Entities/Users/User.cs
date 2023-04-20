using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Intermidiate;
using Domain.Enums;
using Domain.Helpers;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users;

public sealed class User
{
    [NotMapped]
    private const int NameLength = 20;

    [NotMapped]
    private const int EmailLength = 30;

    [NotMapped]
    private const int PasswordLength = 15;

    public UserId Id { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string Password { get; private set; } = string.Empty;

    public HashSet<UserGame> Games { get; private set; } = new();

    public HashSet<Role> Roles { get; private set; } = new();

    public CompanyId? CompanyId { get; private set; }

    public Company? Company { get; private set; }

    public static User? Create(string name, string email,
        string password, List<RoleType> roleTypes)
    {
        if (string.IsNullOrEmpty(name) ||
            string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password))
            return null;

        if (name.Length > NameLength ||
            email.Length > EmailLength ||
            password.Length > PasswordLength)
            return null;

        var passwordHash = PasswordHasher.Hash(password);

        var user = new User
        {
            Name = name,
            Email = email,
            Password = passwordHash
        };

        foreach (var role in roleTypes)
        {
            user.Roles.Add(Role.Create(role));
        }

        return new User();
    }

    public static bool VerifyPassword(string password, string passwordHash)
    {
        return PasswordHasher.Verify(password, passwordHash);
    }

    public void AddRole(RoleType roleType)
    {
        Roles.Add(Role.Create(roleType));
    }

    public void AddGame(Game game)
    {
        Games.Add(UserGame.Create(this, game));
    }

    public bool SetGameAsFavorite(GameId gameId)
    {
        var userGame = Games.FirstOrDefault(game => game.GameId == gameId);

        if (userGame == null)
            return false;

        userGame.SetFavorite();

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
