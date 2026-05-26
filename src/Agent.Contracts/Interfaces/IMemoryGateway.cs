namespace Agent.Contracts.Interfaces;

public interface IMemoryGateway
{
    void Set(string key, object value);

    object? Get(string key);
}
