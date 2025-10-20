using System.ClientModel;
using Microsoft.Agents.AI;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

// Load user secrets from the project
var config = new ConfigurationBuilder().Build();

// Populate values from your OpenAI deployment
var modelId = config["AzureOpenAI:ModelId"] ?? "gpt-4o-demo";
var endpoint = config["AzureOpenAI:Endpoint"] ?? "https://{your-custom-endpoint}.openai.azure.com/";

// Create a service collection to hold the agent plugin and its dependencies.
ServiceCollection services = new();
services.AddSingleton<LightsPlugin>();

IServiceProvider serviceProvider = services.BuildServiceProvider();


AIAgent lightsAgent = new AzureOpenAIClient(
    new Uri(endpoint),
    new DefaultAzureCredential())
    .GetChatClient(modelId)
    .CreateAIAgent(
        instructions: "You answer questions about lights and their states.",
        name: "LightsAgent",
        description: "An agent that answers questions about lights and can control their states.",
        tools: [.. serviceProvider.GetRequiredService<LightsPlugin>().AsAITools()],
        services: serviceProvider
    );

// Create an Agent Thread
AgentThread lightsAgentThread1 = lightsAgent.GetNewThread();

Console.WriteLine("Lights Plugin Agent Demo. Type a message and press Enter to send. Press Enter on an empty line to exit.\n");
Console.WriteLine("\nHere are some example commands you can try:");
Console.WriteLine("\t Light: Is the Entrance light on?");
Console.WriteLine("\t Light: Turn on the Outside light.");
Console.WriteLine("\t Light: Change the color of the Main Stage light to blue.");
Console.WriteLine("\t Light: Set the brightness of the Second Stage light to low.");

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
    ChatCompletion chatCompletion = await lightsAgent.RunAsync([chatMessage], lightsAgentThread1);
    
    // Print the results
    Console.WriteLine("LightsAgent > " + chatCompletion.Content.Last().Text);

} while (userInput is not null);

