using Agent.Contracts.Interfaces;
using Agent.Registry.Agents;
using Agent.Registry.Agents.Industry;

namespace Agent.Registry.Services;

public class AgentRegistry : IAgentRegistry
{
    private readonly List<IAgent> _agents;

    public AgentRegistry()
    {
        _agents =
        [
            new RestaurantLeadAgent(),

            new LeadAgent()
        ];
    }

    public List<IAgent> GetAgents()
    {
        return _agents;
    }
}
