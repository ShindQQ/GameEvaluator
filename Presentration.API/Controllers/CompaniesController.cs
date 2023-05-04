using Application.Common.Requests;
using Application.Companies.Commands.CreateCommand;
using Application.Companies.Commands.DeleteCommand;
using Application.Companies.Commands.Games.CreateCommand;
using Application.Companies.Commands.Games.RemoveGame;
using Application.Companies.Commands.UpdateCommand;
using Application.Companies.Commands.Workers.AddWorker;
using Application.Companies.Commands.Workers.RemoveWorker;
using Application.Companies.Queries;
using Domain.Entities.Companies;
using Domain.Entities.Games;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return Ok(result);
    }

    [HttpPost("/games")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> CreateGameAsync(
        [FromBody] CreateGameRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateGameCommand
        {
            Name = request.Name,
            Description = request.Description,
        }, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);
        await _cache.EvictByTagAsync("games", cancellationToken);

        return NoContent();
    }

    [HttpPost("/{companyId}/games")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<IActionResult> CreateGameAsync(
        [FromBody] CreateGameRequest request,
        [FromRoute] CompanyId companyId,
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

    [HttpDelete("games/{gameId}")]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> RemoveGameAsync(
        [FromRoute] GameId gameId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveGameFromCompanyCommand
        {
            GameId = gameId
        }, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [HttpDelete("/{companyId}/games/{gameId}")]

    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<IActionResult> RemoveGameAsync(
        [FromRoute] GameId gameId,
        [FromRoute] CompanyId companyId,
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

    [Authorize(Roles = "Company")]
    [HttpPut("workers/{workerId}")]
    public async Task<IActionResult> AddWorkerAsync(
        [FromRoute] UserId workerId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddWorkerCommand
        {
            UserId = workerId
        }, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpPut("{companyId}/workers/{workerId}")]
    public async Task<IActionResult> AddWorkerAsync(
       [FromRoute] UserId workerId,
       [FromRoute] CompanyId companyId,
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

    [Authorize(Roles = "Company")]
    [HttpDelete("workers/{workerId}")]
    public async Task<IActionResult> RemoveWorkerAsync(
        [FromRoute] UserId workerId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveWorkerCommand
        {
            UserId = workerId
        }, cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "SuperAdmin, Admin")]
    [HttpDelete("/{companyId}/workers/{workerId}")]
    public async Task<IActionResult> RemoveWorkerAsync(
        [FromRoute] UserId workerId,
        [FromRoute] CompanyId companyId,
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
    [OutputCache(PolicyName = "Companies")]
    [HttpGet("{companyId?}/{pageNumber}/{pageSize}")]
    [AllowAnonymous]
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
    [Authorize(Roles = "SuperAdmin, Admin")]
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
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] CompanyId companyId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCompanyCommand(companyId), cancellationToken);

        await _cache.EvictByTagAsync("companies", cancellationToken);

        return NoContent();
    }
}
