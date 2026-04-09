// A2A as agent
using System.Text.Json;
using A2A;
using Microsoft.Extensions.AI;

// dotnet add package Microsoft.Agents.AI.A2A --version 1.0.0-preview.260402.1

A2ACardResolver agentCardResolver = new A2ACardResolver(new Uri("https://netbc-weather-agent.azurewebsites.net/"));
AgentCard agentCard = await agentCardResolver.GetAgentCardAsync();

JsonSerializerOptions s_indentedOptions = new(A2AJsonUtilities.DefaultOptions){ WriteIndented = true};
Console.WriteLine("\nAgent card details:");
Console.WriteLine(JsonSerializer.Serialize(agentCard, s_indentedOptions));


A2AClient chatClient = new(new Uri(agentCard.Url));
var question = "What is the weather like in Vancouver?";

// Send the message and get the response
var response = await chatClient.AsAIAgent().RunAsync(question);
Console.WriteLine($"Received complete response from agent: {response.Text}\n\n");

// Send the message and stream the response
var streamingResponse = chatClient.AsAIAgent().RunStreamingAsync(question);
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