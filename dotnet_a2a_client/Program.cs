using A2A;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using System.Text.Json;
using A2AJsonUtilities = A2A.A2AJsonUtilities;

// A2A as agent
A2ACardResolver agentCardResolver = new A2ACardResolver(new Uri("https://netbc-weather-agent.azurewebsites.net/"));
AgentCard agentCard = await agentCardResolver.GetAgentCardAsync();

JsonSerializerOptions s_indentedOptions = new(A2AJsonUtilities.DefaultOptions){ WriteIndented = true};
Console.WriteLine("\nAgent card details:");
Console.WriteLine(JsonSerializer.Serialize(agentCard, s_indentedOptions));


A2AClient a2aChatClient = new(new Uri(agentCard.Url));

// Send the message and get the response
Console.WriteLine("\nNon-Streaming Message Communication");
var response = await a2aChatClient.AsAIAgent().RunAsync("What is the weather like in Vancouver?");
Console.WriteLine($" Received complete response from agent: {response.Text}");

var streamingResponse = a2aChatClient.AsAIAgent().RunStreamingAsync("What is the weather like in Vancouver?");
Console.WriteLine("\nStreaming Message Communication"); 
await foreach (var update in streamingResponse)
{
    foreach (var content in update.Contents)
    {
        if (content is TextContent textContent)
        {
            Console.Write(textContent.Text);
        }
    }
}

Console.WriteLine("\n\n");

// A2A as tool ========================================================
AIAgent remoteAgent = await agentCardResolver.GetAIAgentAsync();

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

string? token = config["GitHub:Token"];
string? endpoint = config["GitHub:ApiEndpoint"] ?? "https://models.github.ai/inference";
string? model = config["GitHub:Model"] ?? "openai/gpt-4o-mini";

var chatClient = new OpenAIClient(
    new ApiKeyCredential(token!),
    new OpenAIClientOptions()
    {
        Endpoint = new Uri(endpoint)
    })
    .GetChatClient(model).AsIChatClient();

var agent = chatClient.AsAIAgent(
        name: "Assistant",
        instructions: @"You are a personal assistant. You are concise with your answers.", 
        tools: [remoteAgent.AsAIFunction()]);

var asToolResponse = await agent.RunAsync("What is the weather like in Vancouver?");
Console.WriteLine($"\n\n{asToolResponse.Text}");

// Create their server agent


// Call the server agent here and put into a workflow.