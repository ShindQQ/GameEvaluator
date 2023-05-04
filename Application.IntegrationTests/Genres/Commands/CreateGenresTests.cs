using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Application.Genres.Commands.CreateCommand;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.IntegrationTests.Genres.Commands;

[Collection("Test collection")]
public sealed class CreateGenresTests : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;

    public CreateGenresTests(CustomerApiFactory apiFactory)
    {
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task CreateGenresCommand_Empty_ReturnValidationException()
    {
        var mediator = new Mock<IMediator>();

        var handler = new CreateGenreCommandHandler(Mock.Of<IGenreRepository>());

        var command = new CreateGenreCommand();

        var res = await handler.Handle(command, CancellationToken.None);

        await FluentActions
            .Invoking(() => handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
