using Agent.Contracts.Interfaces.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Agent.Host.Controllers;

[ApiController]
[Route("api/graph")]
public class GraphController : ControllerBase
{
    private readonly IExecutionGraphStore _store;

    public GraphController(IExecutionGraphStore store)
    {
        _store = store;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var graph = await _store.GetGraph();
        return Ok(graph);
    }
}
