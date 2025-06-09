using Accidenta.Application.Accounts.Commands;
using Accidenta.Application.Accounts.DTO;
using Accidenta.Application.Accounts.Queries;
using Accidenta.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        var id = await _mediator.Send(new CreateAccount(request));
        return CreatedAtAction(nameof(GetById), new { id }, new { AccountId = id });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var account = await _mediator.Send(new GetAccountByIdQuery(id));
        return Ok(account);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllAccountsQuery());
        return Ok(result);
    }
}
