
using System.ClientModel;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Reflection;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

// Load user secrets from the project
var config = new ConfigurationBuilder()
.AddUserSecrets<Program>()
.Build();

// Populate values from your OpenAI deployment
var modelId = config["AzureOpenAI:ModelId"] ?? "gpt-4o-demo";
var endpoint = config["AzureOpenAI:Endpoint"] ?? "https://{your-custom-endpoint}.openai.azure.com/";

// Set up the Azure OpenAI client
var chatClient = new AzureOpenAIClient(
    new Uri(endpoint),
    new DefaultAzureCredential())
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