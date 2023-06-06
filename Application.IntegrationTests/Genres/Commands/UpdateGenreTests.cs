using Application.Common.Exceptions;
using Application.Genres.Commands.CreateCommand;
using Application.Genres.Commands.UpdateCommand;
using Application.Genres.Queries;
using Application.IntegrationTests;
using Domain.Entities.Genres;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Genres.Commands;

[Collection("Test collection")]
public sealed class UpdateGenreTests : BaseTestFixture
{
    public UpdateGenreTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task UpdateGenreCommand_NotExistingId_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new UpdateGenreCommand
        {
            Id = new GenreId(Guid.NewGuid())
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task UpdateGenreCommand_ExistingId_ReturnNoResult(
        string name, string description)
    {
        const string updateName = "Updated";
        const string updateDescription = "Updated really big description";

        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var itemId = await mediator!.Send(new CreateGenreCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        await mediator!.Send(new UpdateGenreCommand
        {
            Id = itemId,
            Name = updateName,
            Description = updateDescription
        }, CancellationToken.None);

        var res = await mediator!.Send(new GenreQuery
        {
            Id = itemId,
            PageNumber = 1,
            PageSize = 1,
        });

        var actual = res.Items.First();

        actual.Id.Should().Be(itemId.Value);
        actual.Name.Should().Be(updateName);
        actual.Description.Should().Be(updateDescription);
    }
}
