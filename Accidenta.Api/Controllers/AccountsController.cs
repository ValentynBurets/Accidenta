using Accidenta.Application.Accounts.Commands;
using Accidenta.Application.Accounts.DTO;
using Accidenta.Application.Accounts.Queries;
using Accidenta.Application.Common.DTO;
using Accidenta.Application.Common.Mediator;
using Accidenta.Application.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher,
        ILogger<AccountsController> logger)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        var id = await _commandDispatcher.Dispatch<CreateAccountCommand, Guid>(
            new CreateAccountCommand(request),
            HttpContext.RequestAborted);

        return CreatedAtAction(nameof(GetById), new { id }, new { AccountId = id });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var account = await _queryDispatcher.Dispatch<GetAccountByIdQuery, AccountDto>(
            new GetAccountByIdQuery(id),
            HttpContext.RequestAborted);

        return Ok(account);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AccountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _queryDispatcher.Dispatch<GetAllAccountsQuery, PagedResult<AccountDto>>(
            new GetAllAccountsQuery(page, pageSize),
            HttpContext.RequestAborted);

        return Ok(result);
    }
}
