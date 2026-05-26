using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;

namespace Agent.Registry.Agents;

public class LeadAgent : IAgent
{
    public string Name => "LeadAgent";

    public Task<AgentResult>
    Execute(
        AgentContext context)
    {
        return Task.FromResult(
            new AgentResult
            {
                Success = true,
                Message = "Lead analyzed successfully",

                Data = new
                {
                    Score = 85,
                    Recommendation =
                        "High potential lead"
                }
            });
    }
}
