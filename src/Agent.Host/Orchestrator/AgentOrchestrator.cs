using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;

namespace Agent.Host.Orchestrator;

public class AgentOrchestrator
{
    private readonly IPlanner _planner;

    private readonly IAgentRegistry _registry;

    public AgentOrchestrator(
        IPlanner planner,
        IAgentRegistry registry)
    {
        _planner = planner;

        _registry = registry;
    }

    public async Task<ExecutionPlan>
    Execute(
        Goal goal)
    {
        return await _planner.Create(goal);
    }
}
