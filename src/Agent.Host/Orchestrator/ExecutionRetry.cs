namespace Agent.Host.Orchestrator;

public static class ExecutionRetry
{
    public static async Task<T> Execute<T>(Func<Task<T>> action, int maxRetries = 2)
    {
        Exception? lastError = null;

        for (int i = 0; i <= maxRetries; i++)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                lastError = ex;
                await Task.Delay(100 * (i + 1));
            }
        }

        throw lastError!;
    }
}
