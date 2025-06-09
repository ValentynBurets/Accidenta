using Accidenta.Application.Common.DTO;
using Accidenta.Application.Contacts.Commands;
using Accidenta.Application.Contacts.DTO;
using Accidenta.Application.Contacts.Queries;
using Accidenta.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public ContactsController(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
    {
        var id = await _mediator.Send(new CreateContact(request));
        return CreatedAtAction(nameof(GetById), new { id }, new { ContactId = id });
    }

    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var contact = await _mediator.Send(new GetContactByIdQuery(id));
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

        var contact = await _mediator.Send(new GetContactByEmailQuery(email));
        return Ok(contact);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ContactDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllContactsQuery(page, pageSize));
        return Ok(result);
    }
}
