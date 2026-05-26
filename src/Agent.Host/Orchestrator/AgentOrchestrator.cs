using Agent.Contracts.Interfaces;
using Agent.Contracts.Interfaces.Persistence;
using Agent.Contracts.Models;
using Agent.Contracts.Models.Graph;
using Agent.Workflow.Tracking;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace Agent.Host.Orchestrator;

public class AgentOrchestrator
{
    private readonly IPlanner _planner;
    private readonly IAgentRegistry _registry;
    private readonly IWorkflowRuntime _runtime;
    private readonly IMemoryGateway _memory;
    private readonly ExecutionGraphTracker _graph;

    private static readonly ConcurrentDictionary<string, bool> _executedGoals = new();
    private const int MaxDepth = 3;

    public AgentOrchestrator(
        IPlanner planner,
        IAgentRegistry registry,
        IWorkflowRuntime runtime,
        IMemoryGateway memory,
        ExecutionGraphTracker graph)
    {
        _planner = planner;
        _registry = registry;
        _runtime = runtime;
        _memory = memory;
        _graph = graph;
    }

    public async Task<AgentResult> Execute(Goal goal)
    {
        return await ExecuteInternal(goal, 0);
    }

    private async Task<AgentResult> ExecuteInternal(Goal goal, int depth)
    {
        if (depth > MaxDepth)
        {
            return new AgentResult { Success = false, Message = "Max depth reached" };
        }

        var key = GenerateKey(goal);

        if (_executedGoals.ContainsKey(key))
        {
            return new AgentResult { Success = true, Message = "Duplicate skipped" };
        }

        _executedGoals[key] = true;

        var node = new ExecutionGraphNode
        {
            GoalId = goal.EntityId,
            Status = "Running",
            Timestamp = DateTime.UtcNow
        };

        await _graph.AddOrUpdateNode(node);

        var plan = await _planner.Create(goal);

        var workflowResult = await ExecutionRetry.Execute(() =>
            _runtime.Execute(plan));

        if (!workflowResult.Success)
        {
            node.Status = "Failed";
            await _graph.AddOrUpdateNode(node);
            return new AgentResult { Success = false, Message = workflowResult.Message };
        }

        var agent = _registry.GetAgents()
            .FirstOrDefault(x => x.CanHandle(goal));

        if (agent == null)
        {
            node.Status = "Failed";
            await _graph.AddOrUpdateNode(node);
            return new AgentResult { Success = false, Message = "No agent found" };
        }

        var context = new AgentContext
        {
            WorkflowId = plan.Id,
            Industry = goal.Industry
        };

        var result = await ExecutionRetry.Execute(() =>
            agent.Execute(context));

        node.Status = "Completed";
        node.AgentName = agent.Name;

        await _graph.AddOrUpdateNode(node);

        if (result.NextGoals == null || !result.NextGoals.Any())
        {
            return result;
        }

        var tasks = result.NextGoals.Select(nextGoal =>
            ExecutionThrottle.Run(async () =>
            {
                await _graph.LinkChild(goal.EntityId, nextGoal.EntityId);
                return await ExecuteInternal(nextGoal, depth + 1);
            })
        );

        await Task.WhenAll(tasks);

        return result;
    }

    private string GenerateKey(Goal goal)
    {
        var raw = $"{goal.EntityType}:{goal.EntityId}:{goal.Industry}";
        using var sha = SHA256.Create();
        return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(raw)));
    }
}
