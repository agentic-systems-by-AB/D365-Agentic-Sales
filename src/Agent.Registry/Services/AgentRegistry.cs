using Agent.Contracts.Interfaces;

namespace Agent.Registry.Services;

public class AgentRegistry : IAgentRegistry
{
    private readonly List<IAgent> _agents;

    public AgentRegistry()
    {
        _agents = new List<IAgent>();
    }

    public List<IAgent> GetAgents()
    {
        return _agents;
    }
}
