using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.IntegrationTests;
using Application.Users.Commands.CreateCommand;
using Application.Users.Commands.Roles.AddRole;
using Application.Users.Commands.Roles.RemoveRole;
using Domain.Entities.Users;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Users.Commands.Roles;

[Collection("Test collection")]
public sealed class RolesTests : BaseTestFixture
{
    public RolesTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task AddRoleCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new AddRoleCommand
        {
            UserId = new UserId(Guid.NewGuid()),
            RoleType = RoleType.Company,
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Fact]
    public async Task RemoveRoleCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new RemoveRoleCommand
        {
            UserId = new UserId(Guid.NewGuid()),
            RoleType = RoleType.Company,
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Theory]
    [InlineData("Username", "somemail@gmail.com", "CorPass1")]
    public async Task AddRoleCommand_WithCorrectData_ReturnRoleAdded(
        string name, string email, string password)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var itemId = await mediator!.Send(new CreateUserCommand
        {
            Name = name,
            Email = email,
            Password = password
        }, CancellationToken.None);

        await mediator!.Send(new AddRoleCommand
        {
            UserId = itemId,
            RoleType = RoleType.Company,
        });

        var userRoles = await ((ApplicationDbContext)dbContext).Users
            .Where(user => user.Id == itemId)
            .Select(user =>
            new
            {
                user,
                user.Roles
            }).ToListAsync();

        var roles = userRoles.First().Roles.Where(role => role.Name.Equals(RoleType.Company.ToString()));

        roles.First().Name.Should().Be(RoleType.Company.ToString());
    }

    [Theory]
    [InlineData("Username", "somemail@gmail.com", "CorPass1")]
    public async Task RemoveRoleCommand_WithCorrectData_ReturnRemovedRole(
         string name, string email, string password)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var itemId = await mediator!.Send(new CreateUserCommand
        {
            Name = name,
            Email = email,
            Password = password
        }, CancellationToken.None);

        await mediator!.Send(new AddRoleCommand
        {
            UserId = itemId,
            RoleType = RoleType.Company,
        });

        await mediator!.Send(new RemoveRoleCommand
        {
            UserId = itemId,
            RoleType = RoleType.Company,
        });

        var userRoles = await ((ApplicationDbContext)dbContext).Users
            .Where(user => user.Id == itemId)
            .Select(user =>
            new
            {
                user,
                user.Roles
            }).ToListAsync();

        var roles = userRoles.First().Roles.Where(role => role.Name.Equals(RoleType.Company.ToString()));

        roles.Count().Should().Be(0);
    }
}
