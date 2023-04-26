using Apllication.Common.Interfaces.Repositories;
using Apllication.Infrastructure;
using Domain.Entities.Users;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository
    : BaseRepository<User, UserId>,
    IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<IQueryable<User>> GetAsync()
        => Task.FromResult(Context.Users
            .AsQueryable());

    public async Task<User?> FindByEmailAsync(string email)
        => await Context.Users
        .Include(user => user.Company)
        .Include(user => user.Roles)
        .FirstOrDefaultAsync(user => user.Email.Equals(email));

    public async Task<User?> FindByNameAsync(string name)
        => await Context.Users
        .FirstOrDefaultAsync(user => user.Name.Equals(name));

    public async Task UpdateRefreshTokenAsync(User user, string refreshToken, DateTime expirationTime)
    {
        user.SetRefreshToken(refreshToken, expirationTime);

        await Context.SaveChangesAsync();
    }

    public override async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
        => await Context.Users
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
}
