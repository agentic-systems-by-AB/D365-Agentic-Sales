using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;
using Agent.Memory.Services;

namespace Agent.Host.Orchestrator;

public class AgentOrchestrator
{
    private readonly IPlanner _planner;

    private readonly IAgentRegistry _registry;

    private readonly IWorkflowRuntime _runtime;

    private readonly MemoryStore _memory;

    public AgentOrchestrator(
        IPlanner planner,
        IAgentRegistry registry,
        IWorkflowRuntime runtime,
        MemoryStore memory)
    {
        _planner = planner;

        _registry = registry;

        _runtime = runtime;

        _memory = memory;
    }

    public async Task<AgentResult>
    Execute(
        Goal goal)
    {
        _memory.Set(
            $"goal:{goal.EntityId}",
            goal);

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

        var result =
            await agent.Execute(
                new AgentContext
                {
                    Industry =
                        goal.Industry
                });

        _memory.Set(
            $"result:{goal.EntityId}",
            result);

        return result;
    }
}
