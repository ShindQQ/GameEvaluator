using DotNet.Testcontainers.Builders;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;
using System.Data.Common;
using Testcontainers.MsSql;
using Xunit;

namespace Application.IntegrationTests;

public sealed class CustomerApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
        .WithCleanUp(true)
        .Build();

    private DbConnection _dbConnection = default!;

    private Respawner _respawner = default!;

    private IConfiguration _configuration = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //builder.ConfigureTestServices(services =>
        //{
        //    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
        //    if (descriptor != null) services.Remove(descriptor);

        //    services.AddDbContext<ApplicationDbContext>(options =>
        //    {
        //        options.UseSqlServer(_dbContainer.GetConnectionString());
        //    });

        //    var serviceProvider = services.BuildServiceProvider();

        //    using var scope = serviceProvider.CreateScope();
        //    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //    context.Database.EnsureCreated();
        //});

        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {

            services
                .Remove<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                        builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_configuration.GetConnectionString("DefaultConnection")!);
        //await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        _configuration = Services.GetRequiredService<IConfiguration>();

        _respawner = await Respawner.CreateAsync(_configuration.GetConnectionString("DefaultConnection")!, new RespawnerOptions
        {
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" }
        });

        //await _dbContainer.StartAsync();
        //await Task.Delay(5000);
        //_dbConnection = new SqlConnection(_dbContainer.GetConnectionString());
        //await InitializeRespawnerAsync();
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" }
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
