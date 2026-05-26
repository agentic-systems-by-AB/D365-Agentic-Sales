using Agent.Contracts.Interfaces;
using Agent.Contracts.Interfaces.Persistence;
using Agent.Host.Orchestrator;
using Agent.Memory.LLM;
using Agent.Memory.LLM.Azure;
using Agent.Memory.LLM.OpenAI;
using Agent.Memory.Persistence;
using Agent.Memory.Replay;
using Agent.Memory.Services;
using Agent.Memory.Azure.EventStream;
using Agent.Memory.Eventing;
using Agent.Memory.Tracing;
using Agent.Planner.Services;
using Agent.Registry.Services;
using Agent.Workflow.Services;
using Agent.Workflow.Tracking;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var llmMode = builder.Configuration.GetValue<string>("LLM:Mode") ?? "Local";
var useAzure = builder.Configuration.GetValue<string>("Runtime:Mode") == "Azure";

/* CORE */
builder.Services.AddScoped<IAgentRegistry, AgentRegistry>();
builder.Services.AddScoped<IPlanner, Planner>();
builder.Services.AddScoped<IWorkflowRuntime, WorkflowRuntime>();
builder.Services.AddScoped<AgentOrchestrator>();

/* MEMORY */
builder.Services.AddSingleton<MemoryStore>();
builder.Services.AddSingleton<IMemoryGateway, MemoryGateway>();

/* TRACE STORE (FIX) */
builder.Services.AddSingleton<DecisionTraceStore>();

/* LLM */
if (llmMode == "Azure")
{
    builder.Services.AddSingleton<ILLMClient, AzureOpenAIClient>();
}
else if (llmMode == "OpenAI")
{
    builder.Services.AddSingleton<ILLMClient, OpenAIClient>();
}
else
{
    builder.Services.AddSingleton<ILLMClient, InMemoryLLMClient>();
}

/* GRAPH */
builder.Services.AddSingleton<IExecutionGraphStore, InMemoryExecutionGraphStore>();
builder.Services.AddSingleton<ExecutionGraphTracker>();

/* EVENT REPLAY */
builder.Services.AddSingleton<IEventReplayStore, InMemoryEventReplayStore>();

/* EVENT STREAM */
builder.Services.AddSingleton<IEventStreamStore, AzureEventStreamStore>();

var app = builder.Build();

app.MapControllers();
app.MapGet("/health", () => "running");

app.Run();
