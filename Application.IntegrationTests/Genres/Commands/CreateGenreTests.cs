using Application.Genres.Commands.CreateCommand;
using Domain.Entities.Genres;
using FluentAssertions;
using FluentValidation;
using GameEvaluator.Application.IntegrationTests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Application.IntegrationTests.Genres.Commands;

[Collection("Test collection")]
public sealed class CreateGenreTests : BaseTestFixture
{
    public CreateGenreTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("Name", "Description<20")]
    [InlineData("<3", "Description<20")]
    [InlineData("<3", "Description bigger then 20 characters")]
    public async Task CreateGenreCommand_Empty_ReturnValidationException(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CreateGenreCommand
        {
            Name = name,
            Description = description,
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task CreateGenreCommand_WithNameAndDescription_ReturnCreatedGenresId(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CreateGenreCommand
        {
            Name = name,
            Description = description,
        };

        var itemId = await mediator!.Send(command, CancellationToken.None);

        itemId.Should().NotBeNull();
        itemId.Should().BeOfType<GenreId>();
    }
}
