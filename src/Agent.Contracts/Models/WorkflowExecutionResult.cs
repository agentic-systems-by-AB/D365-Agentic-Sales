namespace Agent.Contracts.Models;

public class WorkflowExecutionResult
{
    public bool Success { get; set; }

    public string Message { get; set; }
        = string.Empty;

    public WorkflowState State { get; set; }
        = new();
}
