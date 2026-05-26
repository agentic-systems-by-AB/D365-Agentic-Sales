using Agent.Contracts.Enums;
using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;

namespace Agent.Workflow.Services;

public class WorkflowRuntime : IWorkflowRuntime
{
    public Task<WorkflowExecutionResult>
    Execute(
        ExecutionPlan plan)
    {
        foreach (var step in plan.Steps)
        {
            step.Completed = true;
        }

        return Task.FromResult(
            new WorkflowExecutionResult
            {
                Success = true,

                Message =
                    "Workflow executed",

                State =
                    new WorkflowState
                    {
                        WorkflowId =
                            plan.Id,

                        Status =
                            WorkflowStatus.Completed,

                        CompletedOn =
                            DateTime.UtcNow
                    }
            });
    }
}
