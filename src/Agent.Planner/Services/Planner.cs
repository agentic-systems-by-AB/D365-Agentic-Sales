using System.Text.Json;
using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;
using Agent.Contracts.Models.LLM;
using Agent.Contracts.Models.Planning;

namespace Agent.Planner.Services;

public class Planner : IPlanner
{
    private readonly IMemoryGateway _memory;
    private readonly ILLMClient _llm;

    public Planner(IMemoryGateway memory, ILLMClient llm)
    {
        _memory = memory;
        _llm = llm;
    }

    public async Task<ExecutionPlan> Create(Goal goal)
    {
        var llmResponse = await BuildLlmPlan(goal);

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

        var subGoals = llmResponse.SubGoals
            .Select(sg => new SubGoal
            {
                Objective = sg,
                EntityType = goal.EntityType
            })
            .ToList();

        return new ExecutionPlan
        {
            Id = Guid.NewGuid().ToString(),
            Steps = steps,
            SubGoals = subGoals
        };
    }

    private async Task<LlmPlanResponse> BuildLlmPlan(Goal goal)
    {
        var prompt =
            $"Return ONLY valid JSON. No explanation.\n" +
            $"Schema:\n" +
            $"{{ \"GoalSummary\": string, \"ReasoningSteps\": string[], \"SubGoals\": string[] }}\n\n" +
            $"Goal: {goal.EntityType}\n" +
            $"Industry: {goal.Industry}";

        var raw = await _llm.Complete(prompt);

        return ParseStrict(raw);
    }

    private LlmPlanResponse ParseStrict(string raw)
    {
        try
        {
            return JsonSerializer.Deserialize<LlmPlanResponse>(raw,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new LlmPlanResponse();
        }
        catch
        {
            return new LlmPlanResponse
            {
                GoalSummary = "Fallback due to invalid JSON",
                ReasoningSteps = { "LLM output invalid" },
                SubGoals = { "Safe Execution Path" }
            };
        }
    }
}
