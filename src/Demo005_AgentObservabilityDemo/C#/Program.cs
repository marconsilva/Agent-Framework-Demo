using System.ClientModel;
using Microsoft.Agents.AI;
using Azure.AI.OpenAI;
using System;
using Azure.Identity;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Configuration;

// Load user secrets from the project
var config = new ConfigurationBuilder()
.AddUserSecrets<Program>()
.Build();

// Populate values from your OpenAI deployment
var modelId = config["AzureOpenAI:ModelId"] ?? "gpt-4o-demo";
var endpoint = config["AzureOpenAI:Endpoint"] ?? "https://{your-custom-endpoint}.openai.azure.com/";

// Create a TracerProvider that exports to the console
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("agent-telemetry-source")
    .AddConsoleExporter()
    .Build();

AIAgent agent = new AzureOpenAIClient(
    new Uri(endpoint),
    new DefaultAzureCredential())
    .GetChatClient(modelId)
    .CreateAIAgent(instructions: "You are good at telling jokes.", name: "Joker")
    .AsBuilder()
    .UseOpenTelemetry(sourceName: "agent-telemetry-source")
    .Build();

// Initiate a back-and-forth chat
Console.Write("Joker Agent Demo. Type a message and press Enter to send. Press Enter on an empty line to exit.\n");
Console.WriteLine("\n\t Joke: Tell me a joke about a pirate.");
Console.WriteLine("\t Joke: Tell me a joke about a AI.");
Console.WriteLine("\t Joke: Tell me a joke about a Portugal.");

string? userInput;
do
{
    // Collect user input
    Console.Write("User > ");
    userInput = Console.ReadLine();

    if (string.IsNullOrEmpty(userInput))
        break;

    UserChatMessage chatMessage = new(userInput);

    // Invoke the agent and output the text result.
    ChatCompletion chatCompletion = await agent.RunAsync([chatMessage]);
    
    // Print the results
    Console.WriteLine("Assistant > " + chatCompletion.Content.Last().Text);

} while (userInput is not null);



// Initiate a back-and-forth chat
Console.WriteLine("\n\n\n");
Console.Write("Switching to streaming mode. Type a message and press Enter to send. Press Enter on an empty line to exit.\n");
Console.WriteLine("\n\t Joke: Tell me a joke about a pirate.");
Console.WriteLine("\t Joke: Tell me a joke about a AI.");
Console.WriteLine("\t Joke: Tell me a joke about a Portugal.");
do
{
    // Collect user input
    Console.Write("User > ");
    userInput = Console.ReadLine();

    if (string.IsNullOrEmpty(userInput))
        break;

    UserChatMessage chatMessage = new(userInput);

    // Print the results
    Console.WriteLine("Assistant > ");

    // Invoke the agent with streaming support.
    AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates = agent.RunStreamingAsync([chatMessage]);
    await foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
    {
        if (completionUpdate.ContentUpdate.Count > 0)
        {
            Console.WriteLine(completionUpdate.ContentUpdate[0].Text);
        }
    }
} while (userInput is not null);