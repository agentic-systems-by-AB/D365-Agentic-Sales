using Agent.Contracts.Interfaces;

namespace Agent.Memory.LLM;

public class InMemoryLLMClient : ILLMClient
{
    public Task<string> Complete(string prompt)
    {
        // Deterministic simulation of LLM reasoning
        var response =
            "Goal Analysis:\n" +
            "- Identify context\n" +
            "- Evaluate domain\n" +
            "- Derive subgoals\n\n" +
            "SubGoals:\n" +
            "- Context Evaluation\n" +
            "- Capability Mapping\n" +
            "- Execution Strategy";

        return Task.FromResult(response);
    }
}
