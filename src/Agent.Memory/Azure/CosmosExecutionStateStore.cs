using Agent.Contracts.Interfaces.Persistence;
using Agent.Contracts.Models.Graph;

namespace Agent.Memory.Azure;

public class CosmosExecutionStateStore : IExecutionGraphStore
{
    public Task SaveNode(ExecutionGraphNode node)
    {
        return Task.CompletedTask;
    }

    public Task SaveEdge(string parentId, string childId)
    {
        return Task.CompletedTask;
    }

    public Task<IReadOnlyDictionary<string, ExecutionGraphNode>> GetGraph()
    {
        return Task.FromResult<IReadOnlyDictionary<string, ExecutionGraphNode>>(
            new Dictionary<string, ExecutionGraphNode>());
    }
}
