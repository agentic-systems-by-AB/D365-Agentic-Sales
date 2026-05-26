using Agent.Contracts.Interfaces;
using Agent.Registry.Agents;

namespace Agent.Registry.Services;

public class AgentRegistry : IAgentRegistry
{
    private readonly List<IAgent> _agents;

    public AgentRegistry()
    {
        _agents =
        [
            new LeadAgent()
        ];
    }

    public List<IAgent> GetAgents()
    {
        return _agents;
    }
}
