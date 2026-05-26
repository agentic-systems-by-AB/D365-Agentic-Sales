using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;

namespace Agent.Host.Orchestrator;

public class AgentOrchestrator
{
    private readonly IPlanner _planner;

    private readonly IAgentRegistry _registry;

    private readonly IWorkflowRuntime _runtime;

    public AgentOrchestrator(
        IPlanner planner,
        IAgentRegistry registry,
        IWorkflowRuntime runtime)
    {
        _planner = planner;

        _registry = registry;

        _runtime = runtime;
    }

    public async Task<AgentResult>
    Execute(
        Goal goal)
    {
        var plan =
            await _planner
            .Create(goal);

        var workflowResult =
            await _runtime
            .Execute(plan);

        if (!workflowResult.Success)
        {
            return new AgentResult
            {
                Success = false,
                Message =
                    workflowResult.Message
            };
        }

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
