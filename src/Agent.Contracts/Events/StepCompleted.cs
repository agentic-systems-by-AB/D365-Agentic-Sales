namespace Agent.Contracts.Events;

public class StepCompleted
{
    public string WorkflowId { get; set; }
        = string.Empty;

    public string StepName { get; set; }
        = string.Empty;

    public DateTime Timestamp { get; set; }
        = DateTime.UtcNow;
}
