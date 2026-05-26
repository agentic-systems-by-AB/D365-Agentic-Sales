using Agent.Contracts.Interfaces;
using Agent.Contracts.Interfaces.Persistence;
using Agent.Host.Orchestrator;
using Agent.Memory.Persistence;
using Agent.Memory.Replay;
using Agent.Memory.Services;
using Agent.Memory.Azure;
using Agent.Memory.Azure.EventStream;
using Agent.Planner.Services;
using Agent.Registry.Services;
using Agent.Workflow.Services;
using Agent.Workflow.Tracking;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var useAzure = builder.Configuration.GetValue<string>("Runtime:Mode") == "Azure";

/* =========================
   CORE SERVICES
========================= */

builder.Services.AddScoped<IAgentRegistry, AgentRegistry>();
builder.Services.AddScoped<IPlanner, Planner>();
builder.Services.AddScoped<IWorkflowRuntime, WorkflowRuntime>();
builder.Services.AddScoped<AgentOrchestrator>();

/* =========================
   MEMORY
========================= */

builder.Services.AddSingleton<MemoryStore>();
builder.Services.AddSingleton<IMemoryGateway, MemoryGateway>();

/* =========================
   GRAPH STORE (STATE)
========================= */

if (useAzure)
{
    builder.Services.AddSingleton<IExecutionGraphStore, CosmosExecutionStateStore>();
    builder.Services.AddSingleton<ExecutionGraphTracker>();
}
else
{
    builder.Services.AddSingleton<IExecutionGraphStore, InMemoryExecutionGraphStore>();
    builder.Services.AddSingleton<ExecutionGraphTracker>();
}

/* =========================
   EVENT REPLAY
========================= */

builder.Services.AddSingleton<IEventReplayStore, InMemoryEventReplayStore>();

/* =========================
   EVENT STREAM (TRANSPORT)
========================= */

if (useAzure)
{
    builder.Services.AddSingleton<IEventStreamStore, AzureEventStreamStore>();
}
else
{
    builder.Services.AddSingleton<IEventStreamStore, AzureEventStreamStore>(); 
    // intentionally same stub until real Event Hub wiring
}

var app = builder.Build();

app.MapControllers();

app.MapGet("/health", () => "running");

app.Run();
