using Aplliction.Users.Queries;
using Application.IntegrationTests;
using Application.Users.Commands.CreateCommand;
using Domain.Entities.Users;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Users.Queries;

[Collection("Test collection")]
public sealed class UserQueryTests : BaseTestFixture
{
    public UserQueryTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task UserQuery_NotExistingId_ReturnEmptyList()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new UserQuery
        {
            Id = new UserId(Guid.NewGuid()),
            PageNumber = 1,
            PageSize = 100
        };

        var res = await mediator!.Send(command, CancellationToken.None);

        res.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task UserQuery_FiveAddedItems_ReturnListWithFiveItems()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        for (int i = 0; i < 5; i++)
        {
            await mediator!.Send(new CreateUserCommand
            {
                Name = $"test{i}",
                Email = $"some{i}@gmail.com",
                Password = $"Pass_{i}"
            }, CancellationToken.None);
        }

        var queryCommand = new UserQuery
        {
            PageNumber = 1,
            PageSize = 100
        };

        var res = await mediator!.Send(queryCommand, CancellationToken.None);

        res.Items.Count.Should().Be(5);
    }
}
