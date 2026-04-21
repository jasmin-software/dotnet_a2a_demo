using System.Text.Json;
using A2A;
using Microsoft.Extensions.AI;

// Get agent card
A2ACardResolver agentCardResolver = new A2ACardResolver(new Uri("https://a2a-weather.azurewebsites.net/"));
AgentCard agentCard = await agentCardResolver.GetAgentCardAsync();

JsonSerializerOptions s_indentedOptions = new(A2AJsonUtilities.DefaultOptions){ WriteIndented = true};
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("\nAgent card details:");
Console.ResetColor();
Console.WriteLine(JsonSerializer.Serialize(agentCard, s_indentedOptions));

// // Create a chat client
// A2AClient chatClient = new(new Uri(agentCard.Url));
// var message = "What is the weather like in Vancouver?";

// // Send message and get the response
// // var response = await chatClient.AsAIAgent().RunAsync(message);
// // Console.WriteLine($"\nReceived complete response from agent: {response.Text}\n");

// // Send message and stream the response
// var streamingResponse = chatClient.AsAIAgent().RunStreamingAsync(message);
// Console.WriteLine("Streaming response from agent:");

// await foreach (var update in streamingResponse)
// {
//     foreach (var content in update.Contents)
//     {
//         if (content is TextContent textContent)
//         {
//             Console.Write(textContent.Text);
//         }
//     }
// }