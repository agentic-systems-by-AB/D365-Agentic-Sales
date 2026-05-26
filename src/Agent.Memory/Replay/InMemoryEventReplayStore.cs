using Agent.Contracts.Interfaces;
using System.Collections.Concurrent;

namespace Agent.Memory.Replay;

public class InMemoryEventReplayStore : IEventReplayStore
{
    private readonly ConcurrentDictionary<string, List<object>> _store = new();

    public Task Save(object @event)
    {
        var workflowId = ExtractWorkflowId(@event);

        if (workflowId == null)
        {
            return Task.CompletedTask;
        }

        if (!_store.ContainsKey(workflowId))
        {
            _store[workflowId] = new List<object>();
        }

        _store[workflowId].Add(@event);

        return Task.CompletedTask;
    }

    public Task<List<object>> GetAll(string workflowId)
    {
        if (_store.TryGetValue(workflowId, out var events))
        {
            return Task.FromResult(events);
        }

        return Task.FromResult(new List<object>());
    }

    private string? ExtractWorkflowId(object @event)
    {
        var prop = @event.GetType().GetProperty("WorkflowId");

        return prop?.GetValue(@event)?.ToString();
    }
}
