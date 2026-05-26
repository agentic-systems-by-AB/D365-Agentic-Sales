using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;
using Agent.Contracts.Models.Planning;

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
        var reasoning = BuildReasoning(goal);

        var steps = new List<WorkflowStep>
        {
            new WorkflowStep { Name = "AnalyzeGoal" },
            new WorkflowStep { Name = "SelectAgent" },
            new WorkflowStep { Name = "Execute" }
        };

        if (_memory.Get($"result:{goal.EntityId}") != null)
        {
            steps.Insert(0, new WorkflowStep { Name = "LoadContext" });
        }

        var subGoals = reasoning.IdentifiedSubGoals
            .Select(sg => new SubGoal
            {
                Objective = sg,
                EntityType = goal.EntityType
            })
            .ToList();

        return Task.FromResult(
            new ExecutionPlan
            {
                Id = Guid.NewGuid().ToString(),
                Steps = steps,
                SubGoals = subGoals
            });
    }

    private PlanReasoning BuildReasoning(Goal goal)
    {
        var reasoning = new PlanReasoning
        {
            GoalSummary = $"Analyze {goal.EntityType} execution for industry {goal.Industry}"
        };

        reasoning.ReasoningSteps.Add("Identify entity context");
        reasoning.ReasoningSteps.Add("Evaluate industry patterns");
        reasoning.ReasoningSteps.Add("Determine required sub-capabilities");

        if (goal.Industry == "Restaurant")
        {
            reasoning.IdentifiedSubGoals.Add("POS Opportunity Evaluation");
            reasoning.IdentifiedSubGoals.Add("Upsell Potential Analysis");
        }
        else
        {
            reasoning.IdentifiedSubGoals.Add("Generic Capability Assessment");
        }

        return reasoning;
    }
}
