namespace Agent.Contracts.Models.LLM;

public class LlmPlanResponse
{
    public string GoalSummary { get; set; } = string.Empty;

    public List<string> ReasoningSteps { get; set; } = new();

    public List<string> SubGoals { get; set; } = new();
}
