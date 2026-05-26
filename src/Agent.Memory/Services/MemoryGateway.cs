using Agent.Contracts.Interfaces;

namespace Agent.Memory.Services;

public class MemoryGateway : IMemoryGateway
{
    private readonly MemoryStore _store;

    public MemoryGateway(MemoryStore store)
    {
        _store = store;
    }

    public void Set(string key, object value)
    {
        _store.Set(key, value);
    }

    public object? Get(string key)
    {
        return _store.Get(key);
    }
}
