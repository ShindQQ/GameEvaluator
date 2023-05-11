using Application.IntegrationTests;
using Application.Platforms.Commands.CreateCommand;
using Application.Platforms.Queries;
using Domain.Entities.Platforms;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Platforms.Queries;

[Collection("Test collection")]
public sealed class PlatformQueryTests : BaseTestFixture
{
    public PlatformQueryTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task PlatformQuery_NotExistingId_ReturnEmptyList()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new PlatformQuery
        {
            Id = new PlatformId(Guid.NewGuid()),
            PageNumber = 1,
            PageSize = 100
        };

        var res = await mediator!.Send(command, CancellationToken.None);

        res.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task PlatformQuery_FiveAddedItems_ReturnListWithFiveItems()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        for (int i = 0; i < 5; i++)
        {
            await mediator!.Send(new CreatePlatformCommand
            {
                Name = $"test{i}",
                Description = $"Really big description number {i}"
            }, CancellationToken.None);
        }

        var res = await mediator!.Send(new PlatformQuery
        {
            PageNumber = 1,
            PageSize = 100
        }, CancellationToken.None);

        res.Items.Count.Should().Be(5);
    }
}
