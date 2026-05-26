namespace Agent.Contracts.Events;

public class WorkflowSuspended
{
    public string WorkflowId { get; set; }
        = string.Empty;

    public string Reason { get; set; }
        = string.Empty;

    public DateTime Timestamp { get; set; }
        = DateTime.UtcNow;
}
