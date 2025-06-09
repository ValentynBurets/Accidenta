using Accidenta.Application.Contacts.Commands;
using Accidenta.Application.Contacts.DTO;
using Accidenta.Application.Contacts.Queries;
using Accidenta.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Accidenta.Api.Controllers;

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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
    {
        try
        {
            var id = await _mediator.Send(new CreateContact(request));
            return CreatedAtAction(nameof(GetById), new { id }, new { ContactId = id });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating contact.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var contact = await _mediator.Send(new GetContactByIdQuery(id));
        return contact == null ? NotFound() : Ok(contact);
    }

    [HttpGet("email")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email is required.");

        var result = await _mediator.Send(new GetContactByEmailQuery(email));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllContactsQuery());
        return Ok(result);
    }
}
