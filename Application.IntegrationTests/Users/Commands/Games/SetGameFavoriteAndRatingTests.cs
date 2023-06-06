using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.Games.CreateCommand;
using Application.IntegrationTests;
using Application.Users.Commands.CreateCommand;
using Application.Users.Commands.Games.AddGame;
using Application.Users.Commands.Games.SetFavorite;
using Application.Users.Commands.Games.SetRating;
using Domain.Entities.Games;
using Domain.Entities.Users;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Users.Commands.Games;

[Collection("Test collection")]
public sealed class SetGameFavoriteAndRatingTests : BaseTestFixture
{
    private Mock<IUserService> _userService = null!;

    public SetGameFavoriteAndRatingTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task SetRatingCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new SetRatingCommand
        {
            UserId = new UserId(Guid.NewGuid()),
            GameId = new GameId(Guid.NewGuid()),
            Rating = 4
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Fact]
    public async Task SetGameAsFavoriteCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new SetGameAsFavoriteCommand
        {
            UserId = new UserId(Guid.NewGuid()),
            GameId = new GameId(Guid.NewGuid()),
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters",
        "Username", "somemail@gmail.com", "CorPass1")]
    public async Task CreateGameCommand_WithCorrectDataAndSuperAdminRole_ReturnRatingAndFavorite(
        string name, string description,
        string userName, string email, string password)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var handler = new CreateGameCommandHandler(
            companyRepository,
            dbContext,
            _userService.Object);

        var companyId = await mediator!.Send(new CreateCompanyCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        var gameId = await handler.Handle(new CreateGameCommand
        {
            CompanyId = companyId,
            Name = name,
            Description = description
        }, CancellationToken.None);

        var userId = await mediator!.Send(new CreateUserCommand
        {
            Name = userName,
            Email = email,
            Password = password
        }, CancellationToken.None);

        await mediator!.Send(new AddGameToUserCommand
        {
            UserId = userId,
            GameId = gameId,
        }, CancellationToken.None);

        await mediator!.Send(new SetGameAsFavoriteCommand
        {
            UserId = userId,
            GameId = gameId,
        }, CancellationToken.None);

        await mediator!.Send(new SetRatingCommand
        {
            UserId = userId,
            GameId = gameId,
            Rating = 4
        }, CancellationToken.None);

        var ratingAndFavoriteState = await ((ApplicationDbContext)dbContext).UserGame
            .Include(user => user.Game)
            .Include(user => user.User)
            .Select(user =>
            new
            {
                user.Rating,
                user.IsFavorite,
            }).FirstAsync();

        ratingAndFavoriteState.Rating.Should().Be(4);
        ratingAndFavoriteState.IsFavorite.Should().Be(true);
    }
}
