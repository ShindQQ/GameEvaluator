using Apllication.Companies.Commands.CreateCommand;
using Apllication.Companies.Commands.DeleteCommand;
using Apllication.Companies.Commands.Games.AddGame;
using Apllication.Companies.Commands.Games.RemoveGame;
using Apllication.Companies.Commands.UpdateCommand;
using Apllication.Companies.Commands.Workers.AddWorker;
using Apllication.Companies.Commands.Workers.RemoveWorker;
using Apllication.Companies.Queries;
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

    [HttpPost("AddGame")]
    public async Task<IActionResult> AddGameAsync([FromBody] AddGameToCompanyCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("RemoveGame")]
    public async Task<IActionResult> RemoveGameAsync([FromBody] RemoveGameFromCompanyCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("AddWorker")]
    public async Task<IActionResult> AddWorkerAsync([FromBody] AddWorkerCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("RemoveWorker")]
    public async Task<IActionResult> RemoveWorkerФsync([FromBody] RemoveWorkerCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpPost("GetCompanies")]
    public async Task<IActionResult> GetAsync([FromBody] CompanyQuery request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateCompanyCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteCompanyCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }
}
