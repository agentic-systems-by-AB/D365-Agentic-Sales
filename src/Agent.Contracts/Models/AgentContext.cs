namespace Agent.Contracts.Models;

public class AgentContext
{
    public string WorkflowId { get; set; }
        = string.Empty;

    public string UserId { get; set; }
        = string.Empty;

    public string Industry { get; set; }
        = string.Empty;

    public Dictionary<string, object>
    Inputs { get; set; } = new();

    public Dictionary<string, object>
    Memory { get; set; } = new();
}
