using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;

namespace Agent.Workflow.Services;

public class WorkflowRuntime : IWorkflowRuntime
{
    public Task Execute(
        ExecutionPlan plan)
    {
        foreach (var step in plan.Steps)
        {
            step.Completed = true;
        }

        return Task.CompletedTask;
    }
}
