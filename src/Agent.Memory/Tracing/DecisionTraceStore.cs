using Agent.Contracts.Models.Tracing;
using System.Collections.Concurrent;

namespace Agent.Memory.Tracing;

public class DecisionTraceStore
{
    private readonly ConcurrentDictionary<string, List<AgentDecisionTrace>> _store = new();

    public void Add(AgentDecisionTrace trace)
    {
        _store.AddOrUpdate(
            trace.WorkflowId,
            _ => new List<AgentDecisionTrace> { trace },
            (_, list) =>
            {
                list.Add(trace);
                return list;
            });
    }

    public List<AgentDecisionTrace> Get(string workflowId)
    {
        _store.TryGetValue(workflowId, out var list);
        return list ?? new List<AgentDecisionTrace>();
    }
}
