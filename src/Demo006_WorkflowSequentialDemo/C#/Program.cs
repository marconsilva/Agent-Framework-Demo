using System.ClientModel;
using Azure.AI.OpenAI;
using Azure.Identity;
using DotNetEnv;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

static ChatClientAgent GetTranslationAgent(string targetLanguage, IChatClient chatClient) =>
    new(chatClient,
        $"You are a translation assistant who only responds in {targetLanguage}. Respond to any " +
        $"input by outputting the name of the input language and then translating the input to {targetLanguage}.");

// Create translation agents for sequential processing
var translationAgents = (from lang in (string[])["Portuguese", "French", "Spanish"]
                         select GetTranslationAgent(lang, chatClient));

// Build sequential workflow
var workflow = AgentWorkflowBuilder.BuildSequential(translationAgents);

// Execute the workflow in streaming mode
var messages = new List<ChatMessage> { new(
    ChatRole.User,
    "I Love visiting Portugal, Lisbon is great, but I prefer Porto!") };

System.Console.WriteLine("Starting Sequential Workflow Demo. \nStreaming results for the input:\n");
Console.WriteLine(string.Join("\n", messages.Select(m => $"{m.Role}: {m.Contents.FirstOrDefault()}")));
Console.WriteLine();

StreamingRun run = await InProcessExecution.StreamAsync(workflow, messages);
await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

List<ChatMessage> result = new();
await foreach (WorkflowEvent evt in run.WatchStreamAsync().ConfigureAwait(false))
{
    if (evt is AgentRunUpdateEvent e)
    {
        Console.WriteLine($"{e.ExecutorId}: {e.Data}");
    }
    else if (evt is WorkflowOutputEvent completed)
    {
        result = (List<ChatMessage>)completed.Data!;
        break;
    }
}

System.Console.WriteLine("\nFinal output from the workflow:\n");

// Display final result
foreach (var message in result)
{
    Console.WriteLine($"{message.Role}: {message.Contents.FirstOrDefault()}");
}