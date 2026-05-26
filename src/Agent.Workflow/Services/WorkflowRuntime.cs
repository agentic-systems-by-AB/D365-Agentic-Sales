using Agent.Contracts.Enums;
using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;
using Agent.Workflow.Tracking;

namespace Agent.Workflow.Services;

public class WorkflowRuntime : IWorkflowRuntime
{
    public Task<WorkflowExecutionResult>
    Execute(
        ExecutionPlan plan)
    {
        var tracker = new WorkflowTracker();

        foreach (var step in plan.Steps)
        {
            step.Completed = true;

            tracker.AddStepResult(
                new WorkflowStepResult
                {
                    StepName = step.Name,
                    Success = true,
                    Message = "Completed",
                    ExecutedOn = DateTime.UtcNow
                });
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
                    },

                StepResults =
                    tracker.GetResults()
                        .ToList()
            });
    }
}
