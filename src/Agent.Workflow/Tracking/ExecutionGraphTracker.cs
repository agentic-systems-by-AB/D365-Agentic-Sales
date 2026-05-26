using System.Collections.Concurrent;

namespace Agent.Workflow.Tracking;

public class ExecutionGraphTracker
{
    private readonly ConcurrentDictionary<string, ExecutionGraphNode> _nodes = new();

    public void AddOrUpdateNode(ExecutionGraphNode node)
    {
        _nodes[node.GoalId] = node;
    }

    public void LinkChild(string parentGoalId, string childGoalId)
    {
        if (_nodes.TryGetValue(childGoalId, out var child))
        {
            child.ParentGoalId = parentGoalId;
        }
        else
        {
            _nodes[childGoalId] = new ExecutionGraphNode
            {
                GoalId = childGoalId,
                ParentGoalId = parentGoalId,
                Status = "Pending",
                Timestamp = DateTime.UtcNow
            };
        }
    }

    public IReadOnlyDictionary<string, ExecutionGraphNode> GetGraph()
    {
        return _nodes;
    }
}
