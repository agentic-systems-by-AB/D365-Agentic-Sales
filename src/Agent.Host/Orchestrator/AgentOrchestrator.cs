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

    public async Task<AgentResult>
    Execute(
        Goal goal)
    {
        await _planner.Create(goal);

        var agent =
            _registry
            .GetAgents()
            .FirstOrDefault(
                x => x.CanHandle(goal));

        if (agent == null)
        {
            return new AgentResult
            {
                Success = false,
                Message =
                    "No matching agent found"
            };
        }

        return await agent.Execute(
            new AgentContext
            {
                Industry =
                    goal.Industry
            });
    }
}
