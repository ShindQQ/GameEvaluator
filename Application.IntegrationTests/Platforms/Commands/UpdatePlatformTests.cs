using Application.Common.Exceptions;
using Application.IntegrationTests;
using Application.Platforms.Commands.CreateCommand;
using Application.Platforms.Commands.UpdateCommand;
using Application.Platforms.Queries;
using Domain.Entities.Platforms;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Platforms.Commands;

[Collection("Test collection")]
public sealed class UpdatePlatformTests : BaseTestFixture
{
    public UpdatePlatformTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task UpdatePlatformCommand_NotExistingId_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new UpdatePlatformCommand
        {
            Id = new PlatformId(Guid.NewGuid())
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task UpdatePlatformCommand_ExistingId_ReturnNoResult(
        string name, string description)
    {
        const string updateName = "Updated";
        const string updateDescription = "Updated really big description";

        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var addCommand = new CreatePlatformCommand
        {
            Name = name,
            Description = description
        };

        var itemId = await mediator!.Send(addCommand, CancellationToken.None);
        var updateCommand = new UpdatePlatformCommand
        {
            Id = itemId,
            Name = updateName,
            Description = updateDescription
        };

        await mediator!.Send(updateCommand, CancellationToken.None);

        var res = await mediator!.Send(new PlatformQuery
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
