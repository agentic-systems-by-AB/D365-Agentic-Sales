namespace Agent.Contracts.Models;

public class WorkflowExecutionContext
{
    public WorkflowState State { get; set; }
        = new();

    public ExecutionPlan Plan { get; set; }
        = new();

    public AgentContext AgentContext { get; set; }
        = new();
}
