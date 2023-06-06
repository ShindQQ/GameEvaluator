using Application.Common.Exceptions;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.DeleteCommand;
using Application.Companies.Queries;
using Application.IntegrationTests;
using Domain.Entities.Companies;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Companies.Commands;

[Collection("Test collection")]
public sealed class DeleteCompanyTests : BaseTestFixture
{
    public DeleteCompanyTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task DeleteCompanyCommand_NotExistingId_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new DeleteCompanyCommand(new CompanyId(Guid.NewGuid()));

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<StatusCodeException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task DeleteCompanyCommand_ExistingId_ReturnNoResult(
        string name, string description)
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var itemId = await mediator!.Send(new CreateCompanyCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);
        var deleteCommand = new DeleteCompanyCommand(itemId);

        await mediator!.Send(deleteCommand, CancellationToken.None);

        var res = await mediator!.Send(new CompanyQuery { Id = itemId });

        res.Items.Should().BeEmpty();
    }
}
