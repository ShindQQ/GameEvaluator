using Application.IntegrationTests;
using Application.Platforms.Commands.CreateCommand;
using Domain.Entities.Platforms;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Platforms.Commands;

[Collection("Test collection")]
public sealed class CreatePlatfromTests : BaseTestFixture
{
    public CreatePlatfromTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("Name", "Description<20")]
    [InlineData("<3", "Description<20")]
    [InlineData("<3", "Description bigger then 20 characters")]
    public async Task CreatePlatformCommand_Empty_ReturnValidationException(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CreatePlatformCommand
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
    public async Task CreatePlatformCommand_WithNameAndDescription_ReturnCreatedGenresId(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CreatePlatformCommand
        {
            Name = name,
            Description = description,
        };

        var itemId = await mediator!.Send(command, CancellationToken.None);

        itemId.Should().NotBeNull();
        itemId.Should().BeOfType<PlatformId>();
    }
}
