using System.Text.Json;
using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;
using Agent.Contracts.Models.LLM;
using Agent.Contracts.Models.Planning;
using Agent.Contracts.Models.Tracing;
using Agent.Memory.Tracing;

namespace Agent.Planner.Services;

public class Planner : IPlanner
{
    private readonly IMemoryGateway _memory;
    private readonly ILLMClient _llm;
    private readonly DecisionTraceStore _traceStore;

    public Planner(
        IMemoryGateway memory,
        ILLMClient llm,
        DecisionTraceStore traceStore)
    {
        _memory = memory;
        _llm = llm;
        _traceStore = traceStore;
    }

    public async Task<ExecutionPlan> Create(Goal goal)
    {
        var (llmResponse, raw) = await BuildLlmPlan(goal);

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
            Steps = BuildSteps(goal),
            SubGoals = subGoals
        };
    }

    private List<WorkflowStep> BuildSteps(Goal goal)
    {
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

        return steps;
    }

    private async Task<(LlmPlanResponse parsed, string raw)> BuildLlmPlan(Goal goal)
    {
        var prompt =
            $"Return ONLY valid JSON.\n" +
            $"Schema: GoalSummary, ReasoningSteps[], SubGoals[]\n" +
            $"Goal: {goal.EntityType}\n" +
            $"Industry: {goal.Industry}";

        var raw = await _llm.Complete(prompt);

        var parsed = Parse(raw);

        _traceStore.Add(new AgentDecisionTrace
        {
            WorkflowId = goal.EntityId,
            Step = "Planner",
            Prompt = prompt,
            Response = raw,
            ReasoningSteps = parsed.ReasoningSteps
        });

        return (parsed, raw);
    }

    private LlmPlanResponse Parse(string raw)
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
                GoalSummary = "Fallback parsing failure",
                ReasoningSteps = { "Invalid JSON from LLM" },
                SubGoals = { "Safe Execution Path" }
            };
        }
    }
}
