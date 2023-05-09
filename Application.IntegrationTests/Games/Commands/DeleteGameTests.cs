using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.Games.CreateCommand;
using Application.Companies.Commands.Games.RemoveGame;
using Application.Games.Queries;
using Application.IntegrationTests;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Games.Commands;

[Collection("Test collection")]
public sealed class DeleteGameTests : BaseTestFixture
{
    private Mock<IUserService> _userService = null!;

    public DeleteGameTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task DeleteGameCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new RemoveGameFromCompanyCommand
        {
            CompanyId = new CompanyId(Guid.NewGuid()),
            GameId = new GameId(Guid.NewGuid()),
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task DeleteGameCommand_WithCorrectDataAndSuperAdminRole_ReturnCreatedGameId(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var createHandler = new CreateGameCommandHandler(
            gameRepository,
            companyRepository,
            dbContext,
            _userService.Object);

        var removeHandler = new RemoveGameFromCompanyCommandHandler(
            companyRepository,
            dbContext,
            _userService.Object);

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

        await removeHandler.Handle(new RemoveGameFromCompanyCommand
        {
            CompanyId = companyId,
            GameId = itemId,
        }, CancellationToken.None);

        var res = await mediator!.Send(new GameQuery { Id = itemId });

        res.Items.Should().BeEmpty();
    }
}
