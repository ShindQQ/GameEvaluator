using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.Workers.AddWorker;
using Application.Companies.Commands.Workers.RemoveWorker;
using Application.IntegrationTests;
using Application.Users.Commands.CreateCommand;
using Domain.Entities.Companies;
using Domain.Entities.Users;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Companies.Commands.Workers;

[Collection("Test collection")]
public sealed class WorkersTests : BaseTestFixture
{
    private Mock<IUserService> _userService = null!;

    public WorkersTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task AddWorkerCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new AddWorkerCommand
        {
            CompanyId = new CompanyId(Guid.NewGuid()),
            UserId = new UserId(Guid.NewGuid()),
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Fact]
    public async Task RemoveWorkerCommand_Empty_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new RemoveWorkerCommand
        {
            CompanyId = new CompanyId(Guid.NewGuid()),
            UserId = new UserId(Guid.NewGuid()),
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters",
        "Username", "somemail@gmail.com", "CorPass1")]
    public async Task AddWorkerCommand_WithCorrectData_ReturnAddedWorker(
        string name, string description,
        string userName, string email, string password)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var handler = new AddWorkerCommandHandler(
            companyRepository,
            userRepository,
            dbContext,
            _userService!.Object);

        var companyId = await mediator!.Send(new CreateCompanyCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        var userId = await mediator!.Send(new CreateUserCommand
        {
            Name = userName,
            Email = email,
            Password = password
        }, CancellationToken.None);

        await handler.Handle(new AddWorkerCommand
        {
            CompanyId = companyId,
            UserId = userId
        }, CancellationToken.None);

        var workers = await ((ApplicationDbContext)dbContext).Companies
            .Include(company => company.Workers)
            .Select(company => company.Workers).ToListAsync();

        workers.Count.Should().Be(1);
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters",
       "Username", "somemail@gmail.com", "CorPass1")]
    public async Task RemoveWorkerCommand_WithCorrectData_ReturnNoWorker(
       string name, string description,
       string userName, string email, string password)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        _userService = new Mock<IUserService>();
        _userService.Setup(e => e.RoleType).Returns(RoleType.SuperAdmin);

        var addHandler = new AddWorkerCommandHandler(
            companyRepository,
            userRepository,
            dbContext,
            _userService!.Object);
        var removeHandler = new RemoveWorkerCommandHandler(
            companyRepository,
            dbContext,
            _userService!.Object);

        var companyId = await mediator!.Send(new CreateCompanyCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        var userId = await mediator!.Send(new CreateUserCommand
        {
            Name = userName,
            Email = email,
            Password = password
        }, CancellationToken.None);

        await addHandler.Handle(new AddWorkerCommand
        {
            CompanyId = companyId,
            UserId = userId
        }, CancellationToken.None);

        await removeHandler.Handle(new RemoveWorkerCommand
        {
            CompanyId = companyId,
            UserId = userId
        }, CancellationToken.None);

        var workers = await ((ApplicationDbContext)dbContext).Companies
            .Include(company => company.Workers)
            .Select(company => company.Workers).ToListAsync();

        workers.First().Count.Should().Be(0);
    }
}
