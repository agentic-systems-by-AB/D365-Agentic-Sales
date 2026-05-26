using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;

namespace Agent.Planner.Services;

public class Planner : IPlanner
{
    private readonly IMemoryGateway _memory;

    public Planner(IMemoryGateway memory)
    {
        _memory = memory;
    }

    public Task<ExecutionPlan> Create(Goal goal)
    {
        var previousResult = _memory.Get($"result:{goal.EntityId}");

        var steps = new List<WorkflowStep>
        {
            new WorkflowStep { Name = "AnalyzeGoal" },
            new WorkflowStep { Name = "SelectAgent" },
            new WorkflowStep { Name = "Execute" }
        };

        if (previousResult != null)
        {
            steps.Insert(0, new WorkflowStep { Name = "LoadContext" });
        }

        var subGoals = new List<SubGoal>();

        if (goal.Industry == "Restaurant")
        {
            subGoals.Add(new SubGoal
            {
                Objective = "POS Opportunity Evaluation",
                EntityType = goal.EntityType
            });
        }

        return Task.FromResult(
            new ExecutionPlan
            {
                Id = Guid.NewGuid().ToString(),
                Steps = steps,
                SubGoals = subGoals
            });
    }
}
