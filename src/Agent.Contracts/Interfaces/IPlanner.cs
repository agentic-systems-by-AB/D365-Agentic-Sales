using Agent.Contracts.Models;

namespace Agent.Contracts.Interfaces;

public interface IPlanner
{
    Task<ExecutionPlan>
    Create(
        Goal goal);
}
