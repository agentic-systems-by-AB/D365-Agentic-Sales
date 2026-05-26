using Agent.Contracts.Models;

namespace Agent.Contracts.Interfaces;

public interface IWorkflowRuntime
{
    Task<WorkflowExecutionResult>
    Execute(
        ExecutionPlan plan);
}
