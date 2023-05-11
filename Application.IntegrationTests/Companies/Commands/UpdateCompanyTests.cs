using Application.Common.Exceptions;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.UpdateCommand;
using Application.Companies.Queries;
using Application.IntegrationTests;
using Domain.Entities.Companies;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Companies.Commands;

[Collection("Test collection")]
public sealed class UpdateCompanyTests : BaseTestFixture
{
    public UpdateCompanyTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task UpdateCompanyCommand_NotExistingId_ReturnNotFoundException()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new UpdateCompanyCommand
        {
            Id = new CompanyId(Guid.NewGuid())
        };

        await FluentActions
            .Invoking(() => mediator!.Send(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("Name", "Description bigger then 20 characters")]
    public async Task UpdateCompanyCommand_ExistingId_ReturnNoResult(
        string name, string description)
    {
        const string updateName = "Updated";
        const string updateDescription = "Updated really big description";

        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var itemId = await mediator!.Send(new CreateCompanyCommand
        {
            Name = name,
            Description = description
        }, CancellationToken.None);

        await mediator!.Send(new UpdateCompanyCommand
        {
            Id = itemId,
            Name = updateName,
            Description = updateDescription
        }, CancellationToken.None);

        var res = await mediator!.Send(new CompanyQuery
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
