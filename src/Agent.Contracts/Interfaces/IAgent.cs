using Agent.Contracts.Models;

namespace Agent.Contracts.Interfaces;

public interface IAgent
{
    string Name { get; }

    bool CanHandle(
        Goal goal);

    Task<AgentResult>
    Execute(
        AgentContext context);
}
