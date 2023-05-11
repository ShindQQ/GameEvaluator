using Application.Genres.Commands.CreateCommand;
using Application.Genres.Queries;
using Application.IntegrationTests;
using Domain.Entities.Genres;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Genres.Queries;

[Collection("Test collection")]
public sealed class GenreQueryTests : BaseTestFixture
{
    public GenreQueryTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task GenreQuery_NotExistingId_ReturnEmptyList()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new GenreQuery
        {
            Id = new GenreId(Guid.NewGuid()),
            PageNumber = 1,
            PageSize = 100
        };

        var res = await mediator!.Send(command, CancellationToken.None);

        res.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GenreQuery_FiveAddedItems_ReturnListWithFiveItems()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        for (int i = 0; i < 5; i++)
        {
            await mediator!.Send(new CreateGenreCommand
            {
                Name = $"test{i}",
                Description = $"Really big description number {i}"
            }, CancellationToken.None);
        }

        var res = await mediator!.Send(new GenreQuery
        {
            PageNumber = 1,
            PageSize = 100
        }, CancellationToken.None);

        res.Items.Count.Should().Be(5);
    }
}
