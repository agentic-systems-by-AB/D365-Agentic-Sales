namespace Agent.Contracts.Events;

public class WorkflowResumed
{
    public string WorkflowId { get; set; }
        = string.Empty;

    public DateTime Timestamp { get; set; }
        = DateTime.UtcNow;
}
