using System.Threading;

namespace Agent.Host.Orchestrator;

public static class ExecutionThrottle
{
    private static readonly SemaphoreSlim _semaphore = new(4); 
    // default concurrency limit = 4

    public static async Task<TResult> Run<TResult>(Func<Task<TResult>> action)
    {
        await _semaphore.WaitAsync();

        try
        {
            return await action();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
