using Agent.Contracts.Interfaces.Persistence;
using Agent.Contracts.Models.Events;
using System.Collections.Concurrent;

namespace Agent.Memory.Eventing;

public class InMemoryEventStreamBroker : IEventStreamStore
{
    private readonly ConcurrentDictionary<string, List<Func<object, Task>>> _subscribers = new();
    private readonly ConcurrentDictionary<string, List<WorkflowEvent>> _store = new();

    public Task Publish(string workflowId, object @event)
    {
        if (@event is WorkflowEvent evt)
        {
            _store.AddOrUpdate(
                workflowId,
                _ => new List<WorkflowEvent> { evt },
                (_, list) =>
                {
                    list.Add(evt);
                    return list;
                });
        }

        if (_subscribers.TryGetValue(workflowId, out var handlers))
        {
            var tasks = handlers.Select(h => h(@event));
            return Task.WhenAll(tasks);
        }

        return Task.CompletedTask;
    }

    public Task Subscribe(string workflowId, Func<object, Task> handler)
    {
        _subscribers.AddOrUpdate(
            workflowId,
            _ => new List<Func<object, Task>> { handler },
            (_, existing) =>
            {
                existing.Add(handler);
                return existing;
            });

        return Task.CompletedTask;
    }

    public Task<List<WorkflowEvent>> GetEvents(string workflowId)
    {
        _store.TryGetValue(workflowId, out var events);
        return Task.FromResult(events ?? new List<WorkflowEvent>());
    }
}
