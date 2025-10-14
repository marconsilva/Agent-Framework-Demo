
using System.ClientModel;
using Azure.AI.OpenAI;
using Azure.Identity;
using DotNetEnv;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Reflection;
using Microsoft.Extensions.AI;


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

// Set up the Azure OpenAI client
var chatClient = new AzureOpenAIClient(
    new Uri(endpoint),
    new ApiKeyCredential(apiKey))
    .GetChatClient(modelId)
    .AsIChatClient();

// Create the executors
ChatClientAgent physicist = new(
    chatClient,
    name: "Physicist",
    instructions: "You are an expert in physics. You answer questions from a physics perspective."
);
ChatClientAgent chemist = new(
    chatClient,
    name: "Chemist",
    instructions: "You are an expert in chemistry. You answer questions from a chemistry perspective."
);
var startExecutor = new ConcurrentStartExecutor();
var aggregationExecutor = new ConcurrentAggregationExecutor();

// Build the workflow by adding executors and connecting them
var workflow = new WorkflowBuilder(startExecutor)
    .AddFanOutEdge(startExecutor, targets: [physicist, chemist])
    .AddFanInEdge(aggregationExecutor, sources: [physicist, chemist])
    .WithOutputFrom(aggregationExecutor)
    .Build();

// Execute the workflow in streaming mode

Console.WriteLine("Concurrent Workflow Demo. \nStreaming results for the input: 'What is temperature?'\n");
await using StreamingRun run = await InProcessExecution.StreamAsync(workflow, "What is temperature?");
await foreach (WorkflowEvent evt in run.WatchStreamAsync().ConfigureAwait(false))
{
    if (evt is WorkflowOutputEvent output)
    {
        Console.WriteLine($"Workflow completed with results:\n{output.Data}");
    }
}