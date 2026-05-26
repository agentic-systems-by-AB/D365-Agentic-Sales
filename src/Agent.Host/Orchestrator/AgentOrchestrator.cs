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
            return new AgentResult
            {
                Success = false,
                Message = "Max execution depth reached"
            };
        }

        var goalKey = GenerateKey(goal);

        if (_executedGoals.ContainsKey(goalKey))
        {
            return new AgentResult
            {
                Success = true,
                Message = "Skipped duplicate goal execution"
            };
        }

        _executedGoals[goalKey] = true;

        _memory.Set($"goal:{goal.EntityId}", goal);

        var node = new ExecutionGraphNode
        {
            GoalId = goal.EntityId,
            Status = "Running",
            Timestamp = DateTime.UtcNow
        };

        await _graph.AddOrUpdateNode(node);

        var plan = await _planner.Create(goal);

        var workflowResult = await _runtime.Execute(plan);

        if (!workflowResult.Success)
        {
            node.Status = "Failed";
            await _graph.AddOrUpdateNode(node);

            return new AgentResult
            {
                Success = false,
                Message = workflowResult.Message
            };
        }

        var agent = _registry.GetAgents()
            .FirstOrDefault(x => x.CanHandle(goal));

        if (agent == null)
        {
            node.Status = "Failed";
            await _graph.AddOrUpdateNode(node);

            return new AgentResult
            {
                Success = false,
                Message = "No matching agent found"
            };
        }

        var context = new AgentContext
        {
            WorkflowId = plan.Id,
            Industry = goal.Industry,
            Inputs =
            {
                ["goal"] = goal
            },
            Memory =
            {
                ["goal"] = _memory.Get($"goal:{goal.EntityId}"),
                ["result"] = _memory.Get($"result:{goal.EntityId}")
            },
            MemoryGateway = _memory
        };

        var result = await agent.Execute(context);

        node.Status = "Completed";
        node.AgentName = agent.Name;

        await _graph.AddOrUpdateNode(node);

        _memory.Set($"result:{goal.EntityId}", result);

        if (result.NextGoals != null && result.NextGoals.Any())
        {
            foreach (var nextGoal in result.NextGoals)
            {
                await _graph.LinkChild(goal.EntityId, nextGoal.EntityId);

                await ExecuteInternal(nextGoal, depth + 1);
            }
        }

        if (plan.SubGoals != null && plan.SubGoals.Any())
        {
            foreach (var subGoal in plan.SubGoals)
            {
                var derivedGoal = new Goal
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityType = subGoal.EntityType,
                    Industry = goal.Industry
                };

                await _graph.LinkChild(goal.EntityId, derivedGoal.EntityId);

                await ExecuteInternal(derivedGoal, depth + 1);
            }
        }

        return result;
    }

    private string GenerateKey(Goal goal)
    {
        var raw = $"{goal.EntityType}:{goal.EntityId}:{goal.Industry}";

        using var sha = SHA256.Create();

        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));

        return Convert.ToBase64String(hash);
    }
}
