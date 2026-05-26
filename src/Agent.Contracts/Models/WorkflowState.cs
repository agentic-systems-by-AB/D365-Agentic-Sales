using Agent.Contracts.Enums;

namespace Agent.Contracts.Models;

public class WorkflowState
{
    public string WorkflowId { get; set; }
        = string.Empty;

    public WorkflowStatus Status { get; set; }

    public DateTime CreatedOn { get; set; }
        = DateTime.UtcNow;

    public DateTime? CompletedOn { get; set; }
}
