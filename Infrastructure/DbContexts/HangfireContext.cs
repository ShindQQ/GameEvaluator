using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public sealed class HangfireContext : DbContext
{
    public HangfireContext(DbContextOptions options) : base(options)
    {
    }
}
