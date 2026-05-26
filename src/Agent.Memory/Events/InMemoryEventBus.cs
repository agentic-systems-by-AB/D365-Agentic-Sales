using Agent.Contracts.Interfaces;

namespace Agent.Memory.Events;

public class InMemoryEventBus : IEventBus
{
    private readonly List<object> _events = new();

    public Task Publish<T>(T @event)
    {
        _events.Add(@event!);
        return Task.CompletedTask;
    }

    public IReadOnlyList<object> GetEvents()
    {
        return _events;
    }

    public IReadOnlyList<T> GetEvents<T>()
    {
        return _events.OfType<T>().ToList();
    }
}
