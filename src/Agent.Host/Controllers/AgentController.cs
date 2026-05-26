using Agent.Contracts.Models;
using Agent.Host.Orchestrator;
using Microsoft.AspNetCore.Mvc;

namespace Agent.Host.Controllers;

[ApiController]
[Route("api/agent")]
public class AgentController : ControllerBase
{
    private readonly AgentOrchestrator _orchestrator;

    public AgentController(
        AgentOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    [HttpPost("execute")]
    public async Task<AgentResult>
    Execute(
        Goal goal)
    {
        return await _orchestrator.Execute(goal);
    }
}
