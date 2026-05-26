using Agent.Contracts.Interfaces;
using Agent.Planner.Services;
using Agent.Registry.Services;
using Agent.Host.Orchestrator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAgentRegistry, AgentRegistry>();

builder.Services.AddScoped<IPlanner, Planner>();

builder.Services.AddScoped<AgentOrchestrator>();

var app = builder.Build();

app.MapGet(
    "/health",
    () => "running");

app.Run();
