using Agent.Contracts.Events;

namespace Agent.Contracts.Interfaces;

public interface IEventReplayStore
{
    Task Save(object @event);

    Task<List<object>> GetAll(string workflowId);
}
