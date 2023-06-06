using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.Games.CreateCommand;
using Application.Games.Commands.Genres.AddGenre;
using Application.Games.Commands.Genres.RemoveGenre;
using Application.Genres.Commands.CreateCommand;
using Application.IntegrationTests;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Genres;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Games.Commands.Genres;

[Collection("Test collection")]
public sealed class GenreTests : BaseTestFixture
{
    private Mock<IUserService> _userService = null!;

    public GenreTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task AddGenreCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        var genreRepository = scope.ServiceProvider.GetRequiredService<IGenreRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var handler = new AddGenreToGameCommandHandler(dbContext, genreRepository, companyRepository, _userService!.Object);

        var command = new AddGenreToGameCommand
        {
            CompanyId = new CompanyId(Guid.NewGuid()),
            GameId = new GameId(Guid.NewGuid()),
            GenreId = new GenreId(Guid.NewGuid()),
        };

        await FluentActions
            .Invoking(() => handler!.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Fact]
    public async Task RemoveGenreCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        var genreRepository = scope.ServiceProvider.GetRequiredService<IGenreRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var handler = new RemoveGenreFromGameCommandHandler(dbContext, genreRepository, companyRepository, _userService!.Object);

        var command = new RemoveGenreFromGameCommand
        {
            CompanyId = new CompanyId(Guid.NewGuid()),
            GameId = new GameId(Guid.NewGuid()),
            GenreId = new GenreId(Guid.NewGuid()),
        };

        await FluentActions
            .Invoking(() => handler!.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task AddGenreCommand_WithCorrectDataAndSuperAdminRole_ReturnAddedGenre(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var genreRepository = scope.ServiceProvider.GetRequiredService<IGenreRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var createHandler = new CreateGameCommandHandler(
            companyRepository,
            dbContext,
        _userService.Object);

        var addHandler = new AddGenreToGameCommandHandler(dbContext, genreRepository, companyRepository, _userService!.Object);

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

        var genreId = await mediator.Send(new CreateGenreCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        await addHandler.Handle(new AddGenreToGameCommand
        {
            CompanyId = companyId,
            GameId = itemId,
            GenreId = genreId
        }, CancellationToken.None);

        var genres = await ((ApplicationDbContext)dbContext).Games
            .Include(game => game.Genres)
            .Select(game => game.Genres).ToListAsync();

        genres.First().Count.Should().Be(1);
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task RemoveGenreCommand_WithCorrectDataAndSuperAdminRole_ReturnNoGenre(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var genreRepository = scope.ServiceProvider.GetRequiredService<IGenreRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var createHandler = new CreateGameCommandHandler(
            companyRepository,
            dbContext,
        _userService.Object);

        var addHandler = new AddGenreToGameCommandHandler(dbContext, genreRepository, companyRepository, _userService!.Object);
        var removeHandler = new RemoveGenreFromGameCommandHandler(dbContext, genreRepository, companyRepository, _userService!.Object);

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

        var genreId = await mediator.Send(new CreateGenreCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        await addHandler.Handle(new AddGenreToGameCommand
        {
            CompanyId = companyId,
            GameId = itemId,
            GenreId = genreId
        }, CancellationToken.None);

        await removeHandler.Handle(new RemoveGenreFromGameCommand
        {
            CompanyId = companyId,
            GameId = itemId,
            GenreId = genreId
        }, CancellationToken.None);

        var genres = await ((ApplicationDbContext)dbContext).Games
            .Include(game => game.Genres)
            .Select(game => game.Genres).ToListAsync();

        genres.First().Count.Should().Be(0);
    }
}
