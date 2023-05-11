using Application.Common.Exceptions;
using Application.Genres.Commands.CreateCommand;
using Application.Genres.Commands.DeleteCommand;
using Application.Genres.Queries;
using Application.IntegrationTests;
using Domain.Entities.Genres;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Genres.Commands;

[Collection("Test collection")]
public sealed class DeleteGenreTests : BaseTestFixture
{
    public DeleteGenreTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task DeleteGenreCommand_NotExistingId_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new DeleteGenreCommand(new GenreId(Guid.NewGuid()));

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task DeleteGenreCommand_ExistingId_ReturnNoResult(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var itemId = await mediator!.Send(new CreateGenreCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        await mediator!.Send(new DeleteGenreCommand(itemId), CancellationToken.None);

        var res = await mediator!.Send(new GenreQuery { Id = itemId });

        res.Items.Should().BeEmpty();
    }
}
