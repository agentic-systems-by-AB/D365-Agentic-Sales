namespace Agent.Contracts.Models.Tracing;

public class AgentDecisionTrace
{
    public string WorkflowId { get; set; } = string.Empty;

    public string Step { get; set; } = string.Empty;

    public string Prompt { get; set; } = string.Empty;

    public string Response { get; set; } = string.Empty;

    public List<string> ReasoningSteps { get; set; } = new();

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
