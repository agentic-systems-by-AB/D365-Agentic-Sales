using Agent.Contracts.Interfaces;
using Agent.Contracts.Interfaces.Persistence;
using Agent.Contracts.Models;
using Agent.Contracts.Models.Events;

namespace Agent.Workflow.Services;

public class WorkflowRuntime : IWorkflowRuntime
{
    private readonly IEventStreamStore _stream;

    public WorkflowRuntime(IEventStreamStore stream)
    {
        _stream = stream;
    }

    public async Task<WorkflowExecutionResult> Execute(ExecutionPlan plan)
    {
        await _stream.Publish(plan.Id, new WorkflowEvent
        {
            WorkflowId = plan.Id,
            Type = "WorkflowStarted",
            Payload = new { plan.Id }
        });

        foreach (var step in plan.Steps)
        {
            await _stream.Publish(plan.Id, new WorkflowEvent
            {
                WorkflowId = plan.Id,
                Type = "StepStarted",
                Payload = new { step.Name }
            });

            await Task.Delay(10);

            await _stream.Publish(plan.Id, new WorkflowEvent
            {
                WorkflowId = plan.Id,
                Type = "StepCompleted",
                Payload = new { step.Name }
            });
        }

        await _stream.Publish(plan.Id, new WorkflowEvent
        {
            WorkflowId = plan.Id,
            Type = "WorkflowCompleted",
            Payload = new { plan.Id }
        });

        return new WorkflowExecutionResult
        {
            Success = true,
            Message = "Workflow executed"
        };
    }
}
