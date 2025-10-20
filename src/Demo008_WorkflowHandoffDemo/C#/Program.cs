using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Agents.AI;
using System.ClientModel;
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

// Create specialized agents
ChatClientAgent historyTutor = new(chatClient,
    "You provide assistance with historical queries. Explain important events and context clearly. Only respond about history.",
    "history_tutor",
    "Specialist agent for historical questions");

ChatClientAgent mathTutor = new(chatClient,
    "You provide help with math problems. Explain your reasoning at each step and include examples. Only respond about math.",
    "math_tutor",
    "Specialist agent for math questions");

ChatClientAgent triageAgent = new(chatClient,
    "You determine which agent to use based on the user's homework question. ALWAYS handoff to another agent.",
    "triage_agent",
    "Routes messages to the appropriate specialist agent");

// Build handoff workflow with routing rules
var workflow = AgentWorkflowBuilder.CreateHandoffBuilderWith(triageAgent)
    .WithHandoffs(triageAgent, new[] { historyTutor, mathTutor }) // Triage can handoff to either specialist    
    .WithHandoff(mathTutor, triageAgent)                 // Math tutor can return to triage
    .WithHandoff(historyTutor, triageAgent)              // History tutor can return to triage
    .Build();

// Process multi-turn conversations
List<ChatMessage> messages = new();

Console.WriteLine("Workflow Handoff Demo. \nEnter a homework question about history or math to begin (type 'exit' to quit):\n");
Console.WriteLine("Examples:");
Console.WriteLine(" - 'Who was the first president of the United States?'");
Console.WriteLine(" - 'What is the Pythagorean theorem?'");
Console.WriteLine(" - 'What is the derivative of x^2?'\n");

Console.Write("Q: ");
string userInput = Console.ReadLine()!;
messages.Add(new(ChatRole.User, userInput));

// Execute workflow and process events
StreamingRun run = await InProcessExecution.StreamAsync(workflow, messages);
await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

List<ChatMessage> newMessages = new();
await foreach (WorkflowEvent evt in run.WatchStreamAsync().ConfigureAwait(false))
{
    if (evt is AgentRunUpdateEvent e)
    {
        Console.WriteLine($"{e.ExecutorId}: {e.Data}");
    }
    else if (evt is WorkflowOutputEvent completed)
    {
        newMessages = (List<ChatMessage>)completed.Data!;
        break;
    }
}


System.Console.WriteLine("\nFinal output from the workflow:\n");

// Display final result
foreach (var message in newMessages)
{
    Console.WriteLine($"{message.Role}: {message.Contents.FirstOrDefault()}");
}