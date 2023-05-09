using Application.IntegrationTests;
using Application.Users.Commands.CreateCommand;
using Domain.Entities.Users;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Users.Commands;

[Collection("Test collection")]
public sealed class CreateUserTests : BaseTestFixture
{
    public CreateUserTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Theory]
    [MemberData(nameof(SetUsernameEmailPasswordData))]
    public async Task CreateUserCommand_Empty_ReturnValidationException(
        string name, string email, string password)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CreateUserCommand
        {
            Name = name,
            Email = email,
            Password = password
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }

    [Theory]
    [InlineData("Username", "somemail@gmail.com", "CorPass1")]
    public async Task CreateUserCommand_WithCorrectData_ReturnCreatedUserId(
        string name, string email, string password)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CreateUserCommand
        {
            Name = name,
            Email = email,
            Password = password
        };

        var itemId = await mediator!.Send(command, CancellationToken.None);

        itemId.Should().NotBeNull();
        itemId.Should().BeOfType<UserId>();
    }
}