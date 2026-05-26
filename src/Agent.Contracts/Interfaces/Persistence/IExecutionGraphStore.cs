using Agent.Contracts.Models.Graph;

namespace Agent.Contracts.Interfaces.Persistence;

public interface IExecutionGraphStore
{
    Task SaveNode(ExecutionGraphNode node);

    Task SaveEdge(string parentGoalId, string childGoalId);

    Task<IReadOnlyDictionary<string, ExecutionGraphNode>> GetGraph();
}
