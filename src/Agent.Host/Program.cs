using Agent.Contracts.Interfaces;
using Agent.Host.Orchestrator;
using Agent.Memory.Events;
using Agent.Memory.Services;
using Agent.Planner.Services;
using Agent.Registry.Services;
using Agent.Workflow.Services;
using Agent.Workflow.Tracking;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IAgentRegistry, AgentRegistry>();
builder.Services.AddScoped<IPlanner, Planner>();
builder.Services.AddScoped<IWorkflowRuntime, WorkflowRuntime>();

builder.Services.AddSingleton<MemoryStore>();
builder.Services.AddSingleton<IMemoryGateway, MemoryGateway>();

builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
builder.Services.AddSingleton<ExecutionGraphTracker>();

builder.Services.AddScoped<AgentOrchestrator>();

var app = builder.Build();

app.MapControllers();

app.MapGet(
    "/health",
    () => "running");

app.Run();
