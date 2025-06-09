using Accidenta.Application.DTO;
using Accidenta.Application.Exceptions;
using Accidenta.Application.Incidents.Commands;
using Accidenta.Application.Incidents.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Accidenta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public IncidentsController(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIncidentRequest request)
    {
        try
        {
            var id = await _mediator.Send(new CreateIncident(request));
            return Ok(new { IncidentId = id });
        }
        catch (NotFoundException)
        {
            return NotFound("Account not found");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating incident.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var incident = await _mediator.Send(new GetIncidentByIdQuery(id));
        return incident == null ? NotFound() : Ok(incident);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllIncidentsQuery());
        return Ok(result);
    }
}
