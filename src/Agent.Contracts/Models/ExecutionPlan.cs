using Agent.Contracts.Models;

namespace Agent.Contracts.Models;

public class ExecutionPlan
{
    public string Id { get; set; }
        = string.Empty;

    public List<WorkflowStep> Steps { get; set; }
        = new();

    public List<SubGoal> SubGoals { get; set; }
        = new();
}
