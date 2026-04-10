using A2A.AspNetCore;
using Microsoft.Extensions.AI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

string githubToken = builder.Configuration["GitHub:Token"]
    ?? throw new InvalidOperationException("GitHub:Token is not set.");

string endpoint = builder.Configuration["GitHub:ApiEndpoint"]
    ?? "https://models.github.ai/inference";

string model = builder.Configuration["GitHub:Model"]
    ?? "openai/gpt-4o-mini";

// Register the chat client
IChatClient chatClient = new OpenAIClient(
    new System.ClientModel.ApiKeyCredential(githubToken),
    new OpenAIClientOptions
    {
        Endpoint = new Uri(endpoint),
    })
    .GetChatClient(model).AsIChatClient();

builder.Services.AddSingleton(chatClient);

// Register calendar store + tools
builder.Services.AddSingleton<ICalendarStore, InMemoryCalendarStore>();


AITool[] tools =
[
    AIFunctionFactory.Create(CalendarTool.GetEventsOnDate),
    AIFunctionFactory.Create(CalendarTool.CreateEvent)
];

// Register an AI agent
var calendarAgent = chatClient.AsAIAgent(
    name: "calendar",
    instructions:
    """
    You are a calendar assistant.
    Help users view and create calendar events.

    Rules:
    - When the user asks what is on a day, use the calendar tools.
    - If a user wants to create an event, gather title, start time, and end time if missing.
    - Keep responses concise and helpful.
    - Always confirm created events with the exact time.

    - Today is always April 21, 2026.
    """,
    tools: tools
);

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// Expose the agent over A2A
app.MapA2A(calendarAgent, path: "/", agentCard: new()
{
    Name = "Calendar Agent",
    Description = "An A2A calendar assistant that can list and create events.",
    Version = "1.0.0"
},
taskManager => app.MapWellKnownAgentCard(taskManager, "/"));

app.Run();