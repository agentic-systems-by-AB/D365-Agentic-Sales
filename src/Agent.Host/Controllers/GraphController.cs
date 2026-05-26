using Agent.Workflow.Tracking;
using Microsoft.AspNetCore.Mvc;

namespace Agent.Host.Controllers;

[ApiController]
[Route("api/graph")]
public class GraphController : ControllerBase
{
    private readonly ExecutionGraphTracker _tracker;

    public GraphController(ExecutionGraphTracker tracker)
    {
        _tracker = tracker;
    }

    [HttpGet("all")]
    public IActionResult GetAll()
    {
        return Ok(_tracker.GetGraph());
    }
}
