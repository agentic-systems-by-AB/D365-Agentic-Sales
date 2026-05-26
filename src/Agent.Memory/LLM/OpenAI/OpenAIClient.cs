using Agent.Contracts.Interfaces;

namespace Agent.Memory.LLM.OpenAI;

public class OpenAIClient : ILLMClient
{
    public async Task<string> Complete(string prompt)
    {
        // TODO: Replace with OpenAI SDK (Responses API)
        // Placeholder keeps architecture stable

        await Task.Delay(50);

        return """
        {
            "GoalSummary": "OpenAI reasoning path",
            "ReasoningSteps": [
                "Interpret goal",
                "Map context",
                "Derive actions"
            ],
            "SubGoals": [
                "Context Analysis",
                "Action Planning",
                "Execution Strategy"
            ]
        }
        """;
    }
}
