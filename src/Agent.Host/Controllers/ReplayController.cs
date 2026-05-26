using Agent.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agent.Host.Controllers;

[ApiController]
[Route("api/replay")]
public class ReplayController : ControllerBase
{
    private readonly IEventReplayStore _replayStore;

    public ReplayController(IEventReplayStore replayStore)
    {
        _replayStore = replayStore;
    }

    [HttpGet("{workflowId}")]
    public async Task<IActionResult> Get(string workflowId)
    {
        var events = await _replayStore.GetAll(workflowId);

        return Ok(new
        {
            WorkflowId = workflowId,
            Events = events
        });
    }
}
