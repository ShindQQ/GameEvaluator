using Apllication.Common.Requests;
using Apllication.Companies.Commands.CreateCommand;
using Apllication.Companies.Commands.DeleteCommand;
using Apllication.Companies.Commands.Games.CreateCommand;
using Apllication.Companies.Commands.Games.RemoveGame;
using Apllication.Companies.Commands.UpdateCommand;
using Apllication.Companies.Commands.Workers.AddWorker;
using Apllication.Companies.Commands.Workers.RemoveWorker;
using Apllication.Companies.Queries;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class CompaniesController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IOutputCacheStore _cache;

    public CompaniesController(IMediator mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return Ok(result);
    }

    [HttpPost("{companyId}/games/")]
    public async Task<IActionResult> CreateGameAsync(
        [FromRoute] CompanyId companyId,
        [FromBody] CreateGameRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateGameCommand
        {
            CompanyId = companyId,
            Name = request.Name,
            Description = request.Description,
        }, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{companyId}/games/{gameId}")]
    public async Task<IActionResult> RemoveGameAsync(
        [FromRoute] CompanyId companyId,
        [FromRoute] GameId gameId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveGameFromCompanyCommand
        {
            CompanyId = companyId,
            GameId = gameId
        }, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [HttpPut("{companyId}/workers/{workerId}")]
    public async Task<IActionResult> AddWorkerAsync(
        [FromRoute] CompanyId companyId,
        [FromRoute] UserId workerId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddWorkerCommand
        {
            CompanyId = companyId,
            UserId = workerId
        }, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{companyId}/workers/{workerId}")]
    public async Task<IActionResult> RemoveWorkerAsync(
        [FromRoute] CompanyId companyId,
        [FromRoute] UserId workerId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveWorkerCommand
        {
            CompanyId = companyId,
            UserId = workerId
        }, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{companyId?}/{pageNumber}/{pageSize}")]
    [OutputCache(PolicyName = "Companies")]
    public async Task<IActionResult> GetAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken,
        CompanyId? companyId = null)
    {
        var result = await _mediator.Send(new CompanyQuery
        {
            Id = companyId,
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{companyId}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] CompanyId companyId,
        [FromBody] UpdateCompanyRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateCompanyCommand
        {
            Id = companyId,
            Name = request.Name,
            Description = request.Description,
        }, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [HttpDelete("{companyId}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] CompanyId companyId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCompanyCommand(companyId), cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }
}
