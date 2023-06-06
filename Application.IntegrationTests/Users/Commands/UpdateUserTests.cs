using Aplliction.Users.Queries;
using Application.Common.Exceptions;
using Application.IntegrationTests;
using Application.Users.Commands.CreateCommand;
using Application.Users.Commands.UpdateCommand;
using Domain.Entities.Users;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Users.Commands;

[Collection("Test collection")]
public sealed class UpdateUserTests : BaseTestFixture
{
    public UpdateUserTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task UpdateUserCommand_NotExistingId_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new UpdateUserCommand
        {
            Id = new UserId(Guid.NewGuid())
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Theory]
    [InlineData("Username", "somemail@gmail.com", "CorPass1")]
    public async Task UpdateUserCommand_ExistingId_ReturnNoResult(
        string name, string email, string password)
    {
        const string updateName = "Usrname";
        const string updateEmail = "somemail1@gmail.com";

        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var itemId = await mediator!.Send(new CreateUserCommand
        {
            Name = name,
            Email = email,
            Password = password
        }, CancellationToken.None);

        await mediator!.Send(new UpdateUserCommand
        {
            Id = itemId,
            Name = updateName,
            Email = updateEmail,
        }, CancellationToken.None);

        var res = await mediator!.Send(new UserQuery
        {
            Id = itemId,
            PageNumber = 1,
            PageSize = 1,
        });

        var actual = res.Items.First();

        actual.Id.Should().Be(itemId.Value);
        actual.Name.Should().Be(updateName);
        actual.Email.Should().Be(updateEmail);
    }
}
