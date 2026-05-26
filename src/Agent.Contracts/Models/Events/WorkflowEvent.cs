namespace Agent.Contracts.Models.Events;

public class WorkflowEvent
{
    public string WorkflowId { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public object? Payload { get; set; }
}
