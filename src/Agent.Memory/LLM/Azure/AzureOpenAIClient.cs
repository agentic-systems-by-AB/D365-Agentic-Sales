using Agent.Contracts.Interfaces;

namespace Agent.Memory.LLM.Azure;

public class AzureOpenAIClient : ILLMClient
{
    public async Task<string> Complete(string prompt)
    {
        // TODO: Replace with real Azure OpenAI SDK call
        // Placeholder keeps architecture intact

        await Task.Delay(50);

        return """
        {
            "GoalSummary": "Azure LLM execution",
            "ReasoningSteps": [
                "Analyze goal context",
                "Apply domain reasoning",
                "Generate subgoals"
            ],
            "SubGoals": [
                "Context Analysis",
                "Opportunity Evaluation",
                "Execution Planning"
            ]
        }
        """;
    }
}
