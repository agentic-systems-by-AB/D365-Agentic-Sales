using Agent.Contracts.Interfaces;
using Agent.Host.Orchestrator;
using Agent.Planner.Services;
using Agent.Registry.Services;
using Agent.Workflow.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IAgentRegistry, AgentRegistry>();

builder.Services.AddScoped<IPlanner, Planner>();

builder.Services.AddScoped<IWorkflowRuntime, WorkflowRuntime>();

builder.Services.AddScoped<AgentOrchestrator>();

var app = builder.Build();

app.MapControllers();

app.MapGet(
    "/health",
    () => "running");

app.Run();
