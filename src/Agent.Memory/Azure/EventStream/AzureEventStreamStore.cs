using Agent.Contracts.Interfaces.Persistence;

namespace Agent.Memory.Azure.EventStream;

public class AzureEventStreamStore : IEventStreamStore
{
    public Task Publish(string workflowId, object @event)
    {
        // TODO: Replace with Azure Service Bus / Event Hub publish
        // Partition key = workflowId

        return Task.CompletedTask;
    }

    public Task Subscribe(string workflowId, Func<object, Task> handler)
    {
        // TODO: Replace with Azure Service Bus / Event Hub consumer
        // Subscription model depends on topic/queue strategy

        return Task.CompletedTask;
    }
}
