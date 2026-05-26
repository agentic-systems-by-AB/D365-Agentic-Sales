namespace Agent.Contracts.Models.Graph;

public class ExecutionGraphNode
{
    public string GoalId { get; set; }
        = string.Empty;

    public string? ParentGoalId { get; set; }

    public string Status { get; set; }
        = "Pending";

    public string? AgentName { get; set; }

    public DateTime Timestamp { get; set; }
        = DateTime.UtcNow;
}
