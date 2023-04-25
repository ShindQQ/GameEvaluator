using Apllication.Common.Requests;
using Apllication.Companies.Commands.CreateCommand;
using Apllication.Companies.Commands.DeleteCommand;
using Apllication.Companies.Commands.Games.AddGame;
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

namespace Presentration.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCompanyCommand request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPut("{companyId}/games/{gameId}")]
    public async Task<IActionResult> AddGameAsync([FromRoute] CompanyId companyId, [FromRoute] GameId gameId)
    {
        await _mediator.Send(new AddGameToCompanyCommand
        {
            CompanyId = companyId,
            GameId = gameId,
        });

        return NoContent();
    }

    [HttpDelete("{companyId}/games/{gameId}")]
    public async Task<IActionResult> RemoveGameAsync([FromRoute] CompanyId companyId, [FromRoute] GameId gameId)
    {
        await _mediator.Send(new RemoveGameFromCompanyCommand { CompanyId = companyId, GameId = gameId });

        return NoContent();
    }

    [HttpPut("{companyId}/workers/{workerId}")]
    public async Task<IActionResult> AddWorkerAsync([FromRoute] CompanyId companyId, [FromRoute] UserId workerId)
    {
        await _mediator.Send(new AddWorkerCommand { CompanyId = companyId, UserId = workerId });

        return NoContent();
    }

    [HttpDelete("{companyId}/workers/{workerId}")]
    public async Task<IActionResult> RemoveWorkerФsync([FromRoute] CompanyId companyId, [FromRoute] UserId workerId)
    {
        await _mediator.Send(new RemoveWorkerCommand { CompanyId = companyId, UserId = workerId });

        return NoContent();
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    [HttpGet("{companyId?}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetAsync(int pageNumber, int pageSize, CompanyId? companyId = null)
    {
        var result = await _mediator.Send(new CompanyQuery
        {
            Id = companyId,
            PageNumber = pageNumber,
            PageSize = pageSize,
        });

        return Ok(result);
    }

    [HttpPatch("{companyId}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] CompanyId companyId, [FromBody] UpdateCompanyRequest request)
    {
        await _mediator.Send(new UpdateCompanyCommand
        {
            Id = companyId,
            Name = request.Name,
            Description = request.Description,
        });

        return NoContent();
    }

    [HttpDelete("{companyId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] CompanyId companyId)
    {
        await _mediator.Send(new DeleteCompanyCommand(companyId));

        return NoContent();
    }
}
