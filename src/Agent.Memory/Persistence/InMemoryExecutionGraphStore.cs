using Agent.Contracts.Interfaces.Persistence;
using Agent.Contracts.Models.Graph;
using System.Collections.Concurrent;

namespace Agent.Memory.Persistence;

public class InMemoryExecutionGraphStore : IExecutionGraphStore
{
    private readonly ConcurrentDictionary<string, ExecutionGraphNode> _nodes = new();

    private readonly ConcurrentDictionary<string, List<string>> _edges = new();

    public Task SaveNode(ExecutionGraphNode node)
    {
        _nodes[node.GoalId] = node;
        return Task.CompletedTask;
    }

    public Task SaveEdge(string parentGoalId, string childGoalId)
    {
        if (!_edges.ContainsKey(parentGoalId))
        {
            _edges[parentGoalId] = new List<string>();
        }

        _edges[parentGoalId].Add(childGoalId);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyDictionary<string, ExecutionGraphNode>> GetGraph()
    {
        return Task.FromResult<IReadOnlyDictionary<string, ExecutionGraphNode>>(_nodes);
    }
}
