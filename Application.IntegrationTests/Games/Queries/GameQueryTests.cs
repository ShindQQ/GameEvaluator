using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.Games.CreateCommand;
using Application.Games.Queries;
using Application.IntegrationTests;
using Domain.Entities.Games;
using Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Games.Queries;

[Collection("Test collection")]
public sealed class GameQueryTests : BaseTestFixture
{
    private Mock<IUserService> _userService = null!;

    public GameQueryTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task GameQuery_NotExistingId_ReturnEmptyList()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new GameQuery
        {
            Id = new GameId(Guid.NewGuid()),
            PageNumber = 1,
            PageSize = 100
        };

        var res = await mediator!.Send(command, CancellationToken.None);

        res.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task CompanyQuery_FiveAddedItems_ReturnListWithFiveItems()
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

        for (int i = 0; i < 5; i++)
        {
            var companyId = await mediator!.Send(new CreateCompanyCommand
            {
                Name = $"name{i}",
                Description = $"Big description really {i}"
            }, CancellationToken.None);

            var itemId = await createHandler.Handle(new CreateGameCommand
            {
                CompanyId = companyId,
                Name = $"name{i}",
                Description = $"Big description really {i}"
            }, CancellationToken.None);
        }

        var queryCommand = new GameQuery
        {
            PageNumber = 1,
            PageSize = 100
        };

        var res = await mediator!.Send(queryCommand, CancellationToken.None);

        res.Items.Count.Should().Be(5);
    }
}
