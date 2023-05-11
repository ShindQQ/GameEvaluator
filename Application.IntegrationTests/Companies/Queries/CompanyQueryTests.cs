using Application.Companies.Commands.CreateCommand;
using Application.Companies.Queries;
using Application.IntegrationTests;
using Domain.Entities.Companies;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameEvaluator.Application.IntegrationTests.Companies.Queries;

[Collection("Test collection")]
public sealed class CompanyQueryTests : BaseTestFixture
{
    public CompanyQueryTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
    }

    [Fact]
    public async Task CompanyQuery_NotExistingId_ReturnEmptyList()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        var command = new CompanyQuery
        {
            Id = new CompanyId(Guid.NewGuid()),
            PageNumber = 1,
            PageSize = 100
        };

        var res = await mediator!.Send(command, CancellationToken.None);

        res.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task CompanyQuery_FiveAddedItems_ReturnListWithFiveItems()
    {
        using var scope = _apiFactory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();

        for (int i = 0; i < 5; i++)
        {
            await mediator!.Send(new CreateCompanyCommand
            {
                Name = $"test{i}",
                Description = $"Really big description number {i}"
            }, CancellationToken.None);
        }

        var res = await mediator!.Send(new CompanyQuery
        {
            PageNumber = 1,
            PageSize = 100
        }, CancellationToken.None);

        res.Items.Count.Should().Be(5);
    }
}
