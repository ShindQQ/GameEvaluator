using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.Games.CreateCommand;
using Application.Games.Commands.Platforms.AddPlatform;
using Application.Games.Commands.Platforms.RemovePlatform;
using Application.IntegrationTests;
using Application.Platforms.Commands.CreateCommand;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Platforms;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Games.Commands.Platforms;

[Collection("Test collection")]
public sealed class PlatformTests : BaseTestFixture
{
    private Mock<IUserService> _userService = null!;

    public PlatformTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task AddPlatformCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        var platformRepository = scope.ServiceProvider.GetRequiredService<IPlatformRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var handler = new AddPlatformCommandToGameHandler(dbContext, platformRepository, _userService!.Object, companyRepository);

        var command = new AddPlatformToGameCommand
        {
            CompanyId = new CompanyId(Guid.NewGuid()),
            GameId = new GameId(Guid.NewGuid()),
            PlatformId = new PlatformId(Guid.NewGuid()),
        };

        await FluentActions
            .Invoking(() => handler!.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task RemovePlatformCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        var platformRepository = scope.ServiceProvider.GetRequiredService<IPlatformRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var handler = new RemovePlatformFromGameCommandHandler(dbContext, platformRepository, companyRepository, _userService!.Object);

        var command = new RemovePlatformFromGameCommand
        {
            CompanyId = new CompanyId(Guid.NewGuid()),
            GameId = new GameId(Guid.NewGuid()),
            PlatformId = new PlatformId(Guid.NewGuid()),
        };

        await FluentActions
            .Invoking(() => handler!.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task AddPlatformCommand_WithCorrectDataAndSuperAdminRole_ReturnAddedPlatform(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        var platformRepository = scope.ServiceProvider.GetRequiredService<IPlatformRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var createHandler = new CreateGameCommandHandler(
            gameRepository,
            companyRepository,
            dbContext,
        _userService.Object);

        var addHandler = new AddPlatformCommandToGameHandler(dbContext, platformRepository, _userService!.Object, companyRepository);

        var companyId = await mediator!.Send(new CreateCompanyCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        var itemId = await createHandler.Handle(new CreateGameCommand
        {
            CompanyId = companyId,
            Name = name,
            Description = description
        }, CancellationToken.None);

        var platform = await mediator.Send(new CreatePlatformCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        await addHandler.Handle(new AddPlatformToGameCommand
        {
            CompanyId = companyId,
            GameId = itemId,
            PlatformId = platform
        }, CancellationToken.None);

        var platforms = await ((ApplicationDbContext)dbContext).Games
            .Include(game => game.Platforms)
            .Select(game => game.Platforms).ToListAsync();

        platforms.First().Count.Should().Be(1);
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task RemovePlatformCommand_WithCorrectDataAndSuperAdminRole_ReturnNoPlatform(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        var platformRepository = scope.ServiceProvider.GetRequiredService<IPlatformRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var createHandler = new CreateGameCommandHandler(
            gameRepository,
            companyRepository,
            dbContext,
        _userService.Object);

        var addHandler = new AddPlatformCommandToGameHandler(dbContext, platformRepository, _userService!.Object, companyRepository);
        var removeHandler = new RemovePlatformFromGameCommandHandler(dbContext, platformRepository, companyRepository, _userService!.Object);

        var companyId = await mediator!.Send(new CreateCompanyCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        var itemId = await createHandler.Handle(new CreateGameCommand
        {
            CompanyId = companyId,
            Name = name,
            Description = description
        }, CancellationToken.None);

        var platformId = await mediator.Send(new CreatePlatformCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        await addHandler.Handle(new AddPlatformToGameCommand
        {
            CompanyId = companyId,
            GameId = itemId,
            PlatformId = platformId
        }, CancellationToken.None);

        await removeHandler.Handle(new RemovePlatformFromGameCommand
        {
            CompanyId = companyId,
            GameId = itemId,
            PlatformId = platformId
        }, CancellationToken.None);

        var platforms = await ((ApplicationDbContext)dbContext).Games
            .Include(game => game.Platforms)
            .Select(game => game.Platforms).ToListAsync();

        platforms.First().Count.Should().Be(0);
    }
}
