namespace Agent.Contracts.Interfaces.Persistence;

public interface IEventStreamStore
{
    Task Append(object @event);

    Task<List<object>> Read(string workflowId);
}
