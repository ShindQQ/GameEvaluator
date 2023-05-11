using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.Games.CreateCommand;
using Application.IntegrationTests;
using Domain.Entities.Games;
using Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Games.Commands;

[Collection("Test collection")]
public sealed class CreateGameTests : BaseTestFixture
{
    private Mock<IUserService> _userService = null!;

    public CreateGameTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Theory]
    [MemberData(nameof(SetNameDescriptionData))]
    public async Task CreateGameCommand_Empty_ReturnValidationException(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CreateGameCommand
        {
            Name = name,
            Description = description
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task CreateGameCommand_WithCorrectDataAndSuperAdminRole_ReturnCreatedGameId(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var handler = new CreateGameCommandHandler(
            gameRepository,
            companyRepository,
            dbContext,
            _userService.Object);

        var companyId = await mediator!.Send(new CreateCompanyCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        var itemId = await handler.Handle(new CreateGameCommand
        {
            CompanyId = companyId,
            Name = name,
            Description = description
        }, CancellationToken.None);

        itemId.Should().NotBeNull();
        itemId.Should().BeOfType<GameId>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters", RoleType.Admin)]
    [InlineData("Name", "Description bigger then 20 characters", RoleType.Company)]
    [InlineData("Name", "Description bigger then 20 characters", RoleType.User)]
    public async Task CreateGameCommand_WithCorrectDataAndRole_ReturnNotFoundException(
        string name, string description, RoleType roleType)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(roleType);

        var handler = new CreateGameCommandHandler(
            gameRepository,
            companyRepository,
            dbContext,
            _userService.Object);

        var companyId = await mediator!.Send(new CreateCompanyCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        var command = new CreateGameCommand
        {
            CompanyId = companyId,
            Name = name,
            Description = description
        };

        await FluentActions
            .Invoking(() => handler!.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }
}
