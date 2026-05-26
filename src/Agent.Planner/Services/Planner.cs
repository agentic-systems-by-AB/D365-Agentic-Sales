using Agent.Contracts.Enums;
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
                new WorkflowStep
                {
                    Type =
                        WorkflowStepType.AnalyzeGoal,

                    Name =
                        "Analyze Goal"
                },

                new WorkflowStep
                {
                    Type =
                        WorkflowStepType.SelectAgent,

                    Name =
                        "Select Agent"
                },

                new WorkflowStep
                {
                    Type =
                        WorkflowStepType.ExecuteAgent,

                    Name =
                        "Execute Agent"
                }
            }
        };

        return Task.FromResult(plan);
    }
}
