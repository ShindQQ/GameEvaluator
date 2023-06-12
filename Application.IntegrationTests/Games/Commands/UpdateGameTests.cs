using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.Games.CreateCommand;
using Application.Games.Commands.UpdateCommand;
using Application.Games.Queries;
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
public sealed class UpdateGameTests : BaseTestFixture
{
    private Mock<IUserService> _userService = null!;

    public UpdateGameTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Theory]
    [MemberData(nameof(SetNameDescriptionData))]
    public async Task UpdateGameCommand_Empty_ReturnValidationException(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new UpdateGameCommand
        {
            Id = new GameId(Guid.NewGuid()),
            Name = name,
            Description = description
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task UpdateGameCommand_WithCorrectDataAndSuperAdminRole_ReturnNoResult(
        string name, string description)
    {
        const string updateName = "Updated";
        const string updateDescription = "Updated really big description";

        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var createHandler = new CreateGameCommandHandler(
            companyRepository,
            dbContext,
            _userService.Object);

        var updateHandler = new UpdateGameCommandHandler(
           gameRepository);

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

        await mediator!.Send(new UpdateGameCommand
        {
            Id = itemId,
            Name = updateName,
            Description = updateDescription
        }, CancellationToken.None);

        var res = await mediator!.Send(new GameQuery
        {
            Id = itemId,
            PageNumber = 1,
            PageSize = 1
        });

        var actual = res.Items.First();

        actual.Id.Should().Be(itemId.Value);
        actual.Name.Should().Be(updateName);
        actual.Description.Should().Be(updateDescription);
    }
}
