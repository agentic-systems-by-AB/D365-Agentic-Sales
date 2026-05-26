using System.Collections.Concurrent;

namespace Agent.Memory.Services;

public class MemoryStore
{
    private readonly ConcurrentDictionary<string, object> _store = new();

    public void Set(
        string key,
        object value)
    {
        _store[key] = value;
    }

    public object? Get(
        string key)
    {
        return _store.TryGetValue(key, out var value)
            ? value
            : null;
    }
}
