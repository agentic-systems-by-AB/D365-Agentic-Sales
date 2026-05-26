using Agent.Contracts.Models;

namespace Agent.Workflow.Tracking;

public class WorkflowTracker
{
    private readonly List<WorkflowStepResult> _results = new();

    public void AddStepResult(
        WorkflowStepResult result)
    {
        _results.Add(result);
    }

    public IReadOnlyList<WorkflowStepResult>
    GetResults()
    {
        return _results;
    }
}
