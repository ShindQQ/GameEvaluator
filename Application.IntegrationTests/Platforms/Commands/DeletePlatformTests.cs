using Application.Common.Exceptions;
using Application.IntegrationTests;
using Application.Platforms.Commands.CreateCommand;
using Application.Platforms.Commands.DeleteCommand;
using Application.Platforms.Queries;
using Domain.Entities.Platforms;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Platforms.Commands;

[Collection("Test collection")]
public sealed class DeletePlatformTests : BaseTestFixture
{
    public DeletePlatformTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task DeletePlatformCommand_NotExistingId_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new DeletePlatformCommand(new PlatformId(Guid.NewGuid()));

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task DeletePlatformCommand_ExistingId_ReturnNoResult(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var addCommand = new CreatePlatformCommand
        {
            Name = name,
            Description = description
        };

        var itemId = await mediator!.Send(addCommand, CancellationToken.None);
        var deleteCommand = new DeletePlatformCommand(itemId);

        await mediator!.Send(deleteCommand, CancellationToken.None);

        var res = await mediator!.Send(new PlatformQuery { Id = itemId });

        res.Items.Should().BeEmpty();
    }
}
