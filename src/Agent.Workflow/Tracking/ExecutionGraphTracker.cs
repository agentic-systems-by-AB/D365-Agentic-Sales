using Agent.Contracts.Interfaces.Persistence;
using Agent.Contracts.Models.Graph;
using System.Collections.Concurrent;

namespace Agent.Workflow.Tracking;

public class ExecutionGraphTracker
{
    private readonly ConcurrentDictionary<string, ExecutionGraphNode> _nodes = new();
    private readonly IExecutionGraphStore _store;

    public ExecutionGraphTracker(IExecutionGraphStore store)
    {
        _store = store;
    }

    public async Task AddOrUpdateNode(ExecutionGraphNode node)
    {
        _nodes[node.GoalId] = node;

        await _store.SaveNode(node);
    }

    public async Task LinkChild(string parentGoalId, string childGoalId)
    {
        if (_nodes.TryGetValue(childGoalId, out var child))
        {
            child.ParentGoalId = parentGoalId;

            await _store.SaveNode(child);
        }
        else
        {
            var node = new ExecutionGraphNode
            {
                GoalId = childGoalId,
                ParentGoalId = parentGoalId,
                Status = "Pending",
                Timestamp = DateTime.UtcNow
            };

            _nodes[childGoalId] = node;

            await _store.SaveNode(node);
        }

        await _store.SaveEdge(parentGoalId, childGoalId);
    }

    public IReadOnlyDictionary<string, ExecutionGraphNode> GetGraph()
    {
        return _nodes;
    }
}
