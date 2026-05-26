namespace Agent.Contracts.Models;

public class WorkflowStepResult
{
    public string StepName { get; set; }
        = string.Empty;

    public bool Success { get; set; }

    public string Message { get; set; }
        = string.Empty;

    public DateTime ExecutedOn { get; set; }
        = DateTime.UtcNow;
}
