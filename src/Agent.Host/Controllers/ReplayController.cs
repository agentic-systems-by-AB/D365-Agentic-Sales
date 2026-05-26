using Agent.Contracts.Interfaces.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Agent.Host.Controllers;

[ApiController]
[Route("api/replay")]
public class ReplayController : ControllerBase
{
    private readonly IEventStreamStore _stream;

    public ReplayController(IEventStreamStore stream)
    {
        _stream = stream;
    }

    [HttpGet("{workflowId}")]
    public async Task<IActionResult> Get(string workflowId)
    {
        // Safe cast only for in-memory implementation
        if (_stream is Agent.Memory.Eventing.InMemoryEventStreamBroker broker)
        {
            var events = await broker.GetEvents(workflowId);
            return Ok(events.OrderBy(e => e.Timestamp));
        }

        return Ok(new
        {
            Message = "Replay not supported for current stream implementation"
        });
    }
}
