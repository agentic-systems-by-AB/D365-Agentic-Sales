using Agent.Contracts.Models;

namespace Agent.Contracts.Interfaces;

public interface IAgent
{
    string Name { get; }

    Task<AgentResult>
    Execute(
        AgentContext context);
}
