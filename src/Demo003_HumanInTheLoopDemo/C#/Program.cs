using System.ClientModel;
using Microsoft.Agents.AI;
using DotNetEnv;
using Azure.AI.OpenAI;
using System;
using Azure.Identity;
using Microsoft.Extensions.AI;
using OpenAI;
using Microsoft.Extensions.DependencyInjection;

// Load environment variables from .env file
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
Env.Load(dotenv);


// Populate values from your OpenAI deployment
var modelId = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-demo";
var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? 
    throw new ArgumentNullException("AZURE_OPENAI_ENDPOINT environment variable is not set");
var apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY") ??
    throw new ArgumentNullException("AZURE_OPENAI_KEY environment variable is not set");
    
// Create a service collection to hold the agent plugin and its dependencies.
ServiceCollection services = new();
services.AddSingleton<LightsPlugin>();

IServiceProvider serviceProvider = services.BuildServiceProvider();


AIAgent lightsAgent = new AzureOpenAIClient(
    new Uri(endpoint),
    new ApiKeyCredential(apiKey))
    .GetChatClient(modelId)
    .CreateAIAgent(
        instructions: "You answer questions about lights and their states.",
        name: "LightsAgent",
        description: "An agent that answers questions about lights and can control their states.",
        tools: [.. serviceProvider.GetRequiredService<LightsPlugin>().AsAITools()],
        services: serviceProvider);


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

    // Invoke the agent and output the text result.
    var response = await lightsAgent.RunAsync(userInput, lightsAgentThread1);

    var userInputRequests = response.UserInputRequests.ToList();


    while (userInputRequests.Count > 0)
    {
        // Ask the user to approve each function call request.
        // For simplicity, we are assuming here that only function approval requests are being made.
        var userInputResponses = userInputRequests
            .OfType<FunctionApprovalRequestContent>()
            .Select(functionApprovalRequest =>
            {
                Console.WriteLine($"The agent would like to invoke the following function, please reply Y to approve: Name {functionApprovalRequest.FunctionCall.Name}");
                return new ChatMessage(ChatRole.User, [functionApprovalRequest.CreateResponse(Console.ReadLine()?.Equals("Y", StringComparison.OrdinalIgnoreCase) ?? false)]);
            })
            .ToList();

        // Pass the user input responses back to the agent for further processing.
        response = await lightsAgent.RunAsync(userInputResponses, lightsAgentThread1);

        userInputRequests = response.UserInputRequests.ToList();
    }

    // Print the results
    Console.WriteLine("LightsAgent > " + response);
} while (userInput is not null);

