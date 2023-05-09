using Application.Companies.Commands.CreateCommand;
using Application.IntegrationTests;
using Domain.Entities.Companies;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Companies.Commands;

[Collection("Test collection")]
public sealed class CreateCompanyTests : BaseTestFixture
{
    public CreateCompanyTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Theory]
    [MemberData(nameof(SetNameDescriptionData))]
    public async Task CreateCompanyCommand_Empty_ReturnValidationException(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CreateCompanyCommand
        {
            Name = name,
            Description = description
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task CreateCompanyCommand_WithCorrectData_ReturnCreatedCompanyId(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CreateCompanyCommand
        {
            Name = name,
            Description = description
        };

        var itemId = await mediator!.Send(command, CancellationToken.None);

        itemId.Should().NotBeNull();
        itemId.Should().BeOfType<CompanyId>();
    }
}
