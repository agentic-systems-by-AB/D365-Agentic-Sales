using Agent.Contracts.Enums;
using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;
using Agent.Contracts.Events;
using Agent.Workflow.Tracking;

namespace Agent.Workflow.Services;

public class WorkflowRuntime : IWorkflowRuntime
{
    private readonly IEventBus _eventBus;

    public WorkflowRuntime(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task<WorkflowExecutionResult>
    Execute(
        ExecutionPlan plan)
    {
        try
        {
            await _eventBus.Publish(
                new WorkflowStarted
                {
                    WorkflowId = plan.Id
                });

            var tracker = new WorkflowTracker();

            foreach (var step in plan.Steps)
            {
                step.Completed = true;

                var stepResult = new WorkflowStepResult
                {
                    StepName = step.Name,
                    Success = true,
                    Message = "Completed",
                    ExecutedOn = DateTime.UtcNow
                };

                tracker.AddStepResult(stepResult);

                await _eventBus.Publish(
                    new StepCompleted
                    {
                        WorkflowId = plan.Id,
                        StepName = step.Name,
                        Timestamp = DateTime.UtcNow
                    });
            }

            var result = new WorkflowExecutionResult
            {
                Success = true,
                Message = "Workflow executed",
                State = new WorkflowState
                {
                    WorkflowId = plan.Id,
                    Status = WorkflowStatus.Completed,
                    CompletedOn = DateTime.UtcNow
                },
                StepResults = tracker.GetResults().ToList()
            };

            await _eventBus.Publish(
                new WorkflowCompleted
                {
                    WorkflowId = plan.Id
                });

            return result;
        }
        catch (Exception ex)
        {
            await _eventBus.Publish(
                new WorkflowFailed
                {
                    WorkflowId = plan.Id,
                    Error = ex.Message,
                    Timestamp = DateTime.UtcNow
                });

            return new WorkflowExecutionResult
            {
                Success = false,
                Message = ex.Message,
                State = new WorkflowState
                {
                    WorkflowId = plan.Id,
                    Status = WorkflowStatus.Failed,
                    CompletedOn = DateTime.UtcNow
                }
            };
        }
    }
}
