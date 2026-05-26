using Agent.Contracts.Interfaces;
using Agent.Contracts.Models;

namespace Agent.Registry.Agents.Industry;

public class RestaurantLeadAgent : IAgent
{
    public string Name =>
        "RestaurantLeadAgent";

    public bool CanHandle(
        Goal goal)
    {
        return goal.EntityType
            .Equals(
                "Lead",
                StringComparison.OrdinalIgnoreCase)

            &&

            goal.Industry
            .Equals(
                "Restaurant",
                StringComparison.OrdinalIgnoreCase);
    }

    public Task<AgentResult>
    Execute(
        AgentContext context)
    {
        return Task.FromResult(
            new AgentResult
            {
                Success = true,

                Message =
                    "Restaurant lead analyzed",

                Data = new
                {
                    Recommendation =
                        "POS Upsell Candidate",

                    Score = 97
                }
            });
    }
}
