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
        => Task.FromResult(Context.User
            .Include(user => user.Games)
            .Include(user => user.Company)
            .Include(user => user.Roles)
            .AsQueryable());

    public override async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
        => await Context.User
            .Include(user => user.Games)
            .Include(user => user.Company)
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
}
