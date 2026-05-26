using Agent.Contracts.Enums;
using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;
using Agent.Contracts.Events;
using Agent.Workflow.Tracking;

namespace Agent.Workflow.Services;

public class WorkflowRuntime : IWorkflowRuntime
{
    private readonly IEventBus _eventBus;
    private readonly IEventReplayStore _replayStore;

    public WorkflowRuntime(
        IEventBus eventBus,
        IEventReplayStore replayStore)
    {
        _eventBus = eventBus;
        _replayStore = replayStore;
    }

    public async Task<WorkflowExecutionResult>
    Execute(
        ExecutionPlan plan)
    {
        try
        {
            var started = new WorkflowStarted
            {
                WorkflowId = plan.Id
            };

            await _eventBus.Publish(started);
            await _replayStore.Save(started);

            var tracker = new WorkflowTracker();

            foreach (var step in plan.Steps)
            {
                step.Completed = true;

                var stepEvent = new StepCompleted
                {
                    WorkflowId = plan.Id,
                    StepName = step.Name,
                    Timestamp = DateTime.UtcNow
                };

                tracker.AddStepResult(
                    new WorkflowStepResult
                    {
                        StepName = step.Name,
                        Success = true,
                        Message = "Completed",
                        ExecutedOn = DateTime.UtcNow
                    });

                await _eventBus.Publish(stepEvent);
                await _replayStore.Save(stepEvent);
            }

            var completed = new WorkflowCompleted
            {
                WorkflowId = plan.Id
            };

            await _eventBus.Publish(completed);
            await _replayStore.Save(completed);

            return new WorkflowExecutionResult
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
        }
        catch (Exception ex)
        {
            var failed = new WorkflowFailed
            {
                WorkflowId = plan.Id,
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };

            await _eventBus.Publish(failed);
            await _replayStore.Save(failed);

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
