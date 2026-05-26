namespace Agent.Contracts.Events;

public class WorkflowFailed
{
    public string WorkflowId { get; set; }
        = string.Empty;

    public string Error { get; set; }
        = string.Empty;

    public DateTime Timestamp { get; set; }
        = DateTime.UtcNow;
}
