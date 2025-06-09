using Accidenta.Application.Accounts.Commands;
using Accidenta.Application.Accounts.Queries;
using Accidenta.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Accidenta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public AccountsController(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        try
        {
            var id = await _mediator.Send(new CreateAccount(request));
            return CreatedAtAction(nameof(GetById), new { id }, new { AccountId = id });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating account.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var account = await _mediator.Send(new GetAccountByIdQuery(id));
        return account == null ? NotFound() : Ok(account);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllAccountsQuery());
        return Ok(result);
    }
}
