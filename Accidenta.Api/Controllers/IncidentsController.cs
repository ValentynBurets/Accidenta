using Accidenta.Application.Common.DTO;
using Accidenta.Application.Common.Mediator;
using Accidenta.Application.DTO;
using Accidenta.Application.Incidents.Commands;
using Accidenta.Application.Incidents.DTO;
using Accidenta.Application.Incidents.Queries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ILogger<IncidentsController> _logger;

    public IncidentsController(
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher,
        ILogger<IncidentsController> logger)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateIncidentRequest request)
    {
        var id = await _commandDispatcher.Dispatch<CreateIncidentCommand, Guid>(
            new CreateIncidentCommand(request),
            HttpContext.RequestAborted);

        return Ok(new { IncidentId = id });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IncidentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var incident = await _queryDispatcher.Dispatch<GetIncidentByIdQuery, IncidentDto>(
            new GetIncidentByIdQuery(id),
            HttpContext.RequestAborted);

        return Ok(incident);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<IncidentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _queryDispatcher.Dispatch<GetAllIncidentsQuery, PagedResult<IncidentDto>>(
            new GetAllIncidentsQuery(page, pageSize),
            HttpContext.RequestAborted);

        return Ok(result);
    }
}
