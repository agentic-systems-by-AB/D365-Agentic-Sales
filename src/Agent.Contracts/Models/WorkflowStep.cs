using Agent.Contracts.Enums;

namespace Agent.Contracts.Models;

public class WorkflowStep
{
    public WorkflowStepType Type { get; set; }

    public string Name { get; set; }
        = string.Empty;

    public bool Completed { get; set; }
}
