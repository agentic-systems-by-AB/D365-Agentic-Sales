namespace Agent.Contracts.Interfaces;

public interface IEventBus
{
    Task Publish<T>(T @event);
}
