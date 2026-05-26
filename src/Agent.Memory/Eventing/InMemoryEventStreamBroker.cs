using Agent.Contracts.Interfaces.Persistence;
using System.Collections.Concurrent;

namespace Agent.Memory.Eventing;

public class InMemoryEventStreamBroker : IEventStreamStore
{
    private readonly ConcurrentDictionary<string, List<Func<object, Task>>> _subscribers = new();

    public Task Publish(string workflowId, object @event)
    {
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
}
