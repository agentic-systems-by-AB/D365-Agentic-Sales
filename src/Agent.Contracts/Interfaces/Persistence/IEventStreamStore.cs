namespace Agent.Contracts.Interfaces.Persistence;

public interface IEventStreamStore
{
    Task Publish(string workflowId, object @event);

    Task Subscribe(string workflowId, Func<object, Task> handler);
}
