namespace Agent.Contracts.Models;

public class ExecutionPlan
{
    public string Id { get; set; }
        = Guid.NewGuid().ToString();

    public Goal Goal { get; set; }
        = new();

    public List<string> Steps { get; set; }
        = new();
}
