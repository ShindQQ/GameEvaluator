using DotNet.Testcontainers.Builders;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Data.Common;
using Testcontainers.MsSql;
using Xunit;

namespace Application.IntegrationTests;

public sealed class CustomerApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithCleanUp(true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
        .Build();

    private DbConnection _dbConnection = default!;

    private Respawner _respawner = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbContainer.GetConnectionString());
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _dbConnection = new SqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawnerAsync();
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
