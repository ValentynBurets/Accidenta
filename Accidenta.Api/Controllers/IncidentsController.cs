using Accidenta.Application.Common.DTO;
using Accidenta.Application.DTO;
using Accidenta.Application.Incidents.Commands;
using Accidenta.Application.Incidents.DTO;
using Accidenta.Application.Incidents.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AccountsController> _logger;

    public IncidentsController(IMediator mediator, ILogger<AccountsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateIncidentRequest request)
    {
        var id = await _mediator.Send(new CreateIncident(request));
        return Ok(new { IncidentId = id });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IncidentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var incident = await _mediator.Send(new GetIncidentByIdQuery(id));
        return Ok(incident);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<IncidentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetAllIncidentsQuery(page, pageSize));
        return Ok(result);
    }
}
