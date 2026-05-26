using Agent.Contracts.Interfaces.Persistence;
using Agent.Contracts.Models.Graph;
using Microsoft.Azure.Cosmos;

namespace Agent.Memory.Azure;

public class CosmosExecutionStateStore : IExecutionGraphStore
{
    private readonly Container _container;

    public CosmosExecutionStateStore(CosmosClient client)
    {
        _container = client.GetContainer("agent-db", "execution-graph");
    }

    public async Task SaveNode(ExecutionGraphNode node)
    {
        node.Status ??= "Unknown";

        await _container.UpsertItemAsync(
            node,
            new PartitionKey(node.GoalId));
    }

    public async Task SaveEdge(string parentId, string childId)
    {
        var edge = new
        {
            id = $"{parentId}->{childId}",
            parentId,
            childId,
            type = "edge"
        };

        await _container.UpsertItemAsync(
            edge,
            new PartitionKey(parentId));
    }

    public async Task<IReadOnlyDictionary<string, ExecutionGraphNode>> GetGraph()
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.type != 'edge'");

        var iterator = _container.GetItemQueryIterator<ExecutionGraphNode>(query);

        var result = new Dictionary<string, ExecutionGraphNode>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();

            foreach (var item in response)
            {
                result[item.GoalId] = item;
            }
        }

        return result;
    }
}
