using Accidenta.Application.Common.DTO;
using Accidenta.Application.Common.Mediator;
using Accidenta.Application.Contacts.Commands;
using Accidenta.Application.Contacts.DTO;
using Accidenta.Application.Contacts.Queries;
using Accidenta.Application.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher,
        ILogger<ContactsController> logger)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
    {
        var id = await _commandDispatcher.Dispatch<CreateContactCommand, Guid>(
            new CreateContactCommand(request),
            HttpContext.RequestAborted);

        return CreatedAtAction(nameof(GetById), new { id }, new { ContactId = id });
    }

    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var contact = await _queryDispatcher.Dispatch<GetContactByIdQuery, ContactDto>(
            new GetContactByIdQuery(id),
            HttpContext.RequestAborted);

        return Ok(contact);
    }

    [HttpGet("email")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email is required.");

        var contact = await _queryDispatcher.Dispatch<GetContactByEmailQuery, ContactDto>(
            new GetContactByEmailQuery(email),
            HttpContext.RequestAborted);

        return Ok(contact);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ContactDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _queryDispatcher.Dispatch<GetAllContactsQuery, PagedResult<ContactDto>>(
            new GetAllContactsQuery(page, pageSize),
            HttpContext.RequestAborted);

        return Ok(result);
    }
}
