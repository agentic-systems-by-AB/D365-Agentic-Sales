namespace Agent.Contracts.Models.Planning;

public class PlanReasoning
{
    public string GoalSummary { get; set; } = string.Empty;

    public List<string> ReasoningSteps { get; set; } = new();

    public List<string> IdentifiedSubGoals { get; set; } = new();
}
