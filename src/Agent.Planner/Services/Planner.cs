using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;

namespace Agent.Planner.Services;

public class Planner : IPlanner
{
    public Task<ExecutionPlan>
    Create(Goal goal)
    {
        var plan = new ExecutionPlan
        {
            Goal = goal,
            Steps =
            {
                "AnalyzeGoal",
                "SelectAgent",
                "Execute"
            }
        };

        return Task.FromResult(plan);
    }
}
