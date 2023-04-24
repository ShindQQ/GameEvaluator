using Apllication.Companies.Commands.CreateCommand;
using Apllication.Companies.Commands.DeleteCommand;
using Apllication.Companies.Commands.UpdateCommand;
using Apllication.Companies.Queries;
using Apllication.Games.Commands.CreateCommand;
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

    [HttpPost("CreateGame")]
    public async Task<IActionResult> CreateGameAsync([FromBody] CreateGameCommand request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
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
