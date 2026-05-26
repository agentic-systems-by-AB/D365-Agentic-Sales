using Agent.Contracts.Interfaces.Persistence;

namespace Agent.Memory.Azure.EventStream;

public class AzureEventStreamStore : IEventStreamStore
{
    public Task Append(object @event)
    {
        // TODO: Replace with Azure Event Hub / Service Bus publish
        return Task.CompletedTask;
    }

    public Task<List<object>> Read(string workflowId)
    {
        // Event Hub is not replay-oriented
        // Typically paired with storage (Cosmos / Blob / ReplayStore)
        return Task.FromResult(new List<object>());
    }
}
