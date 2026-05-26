namespace Agent.Contracts.Interfaces;

public interface ILLMClient
{
    Task<string> Complete(string prompt);
}
