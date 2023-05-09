using Aplliction.Users.Queries;
using Application.Common.Exceptions;
using Application.IntegrationTests;
using Application.Users.Commands.CreateCommand;
using Application.Users.Commands.DeleteCommand;
using Domain.Entities.Users;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Users.Commands;

[Collection("Test collection")]
public sealed class DeleteUserTests : BaseTestFixture
{
    public DeleteUserTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task DeleteUserCommand_NotExistingId_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new DeleteUserCommand(new UserId(Guid.NewGuid()));

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("Username", "somemail@gmail.com", "CorPass1")]
    public async Task DeleteUserCommand_ExistingId_ReturnNoResult(
        string name, string email, string password)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var addCommand = new CreateUserCommand
        {
            Name = name,
            Email = email,
            Password = password
        };

        var itemId = await mediator!.Send(addCommand, CancellationToken.None);
        var deleteCommand = new DeleteUserCommand(itemId);

        await mediator!.Send(deleteCommand, CancellationToken.None);

        var res = await mediator!.Send(new UserQuery { Id = itemId });

        res.Items.Should().BeEmpty();
    }
}
