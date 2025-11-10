
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Reflection;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.ClientModel;
using System.Text;

namespace WorkflowSequentialDemo.Web.Components.Contollers
{
    public class WorkflowController
    {
        private readonly ILogger<WorkflowController> _logger;
        private readonly ChatClientAgent _physicist;
        private readonly ChatClientAgent _chemist;
        private readonly IChatClient _chatClient;
        private readonly Workflow _workflow;
        private readonly IEnumerable<ChatClientAgent> _translationAgents;


        public WorkflowController(ILogger<WorkflowController> logger, IConfiguration config)
        {
            _logger = logger;

            string endpoint = config["AzureOpenAI:Endpoint"] ?? throw new NullReferenceException("AzureOpenAI:Endpoint is null");
            string modelId = config["AzureOpenAI:ModelId"] ?? throw new NullReferenceException("AzureOpenAI:ModelId is null");

            _logger.LogInformation("Initializing WorkflowSequential Controller with Endpoint: {Endpoint} and ModelId: {ModelId}", endpoint, modelId);

            // Set up the Azure OpenAI client
            _chatClient = new AzureOpenAIClient(
                new Uri(endpoint),
                new DefaultAzureCredential())
                .GetChatClient(modelId)
                .AsIChatClient();

            _translationAgents = (from lang in (string[])["Portuguese", "French", "Spanish"]
                                  select GetTranslationAgent(lang, _chatClient));

            // Build sequential workflow
            _workflow = AgentWorkflowBuilder.BuildSequential(_translationAgents);
        }

        private static ChatClientAgent GetTranslationAgent(string targetLanguage, IChatClient chatClient) =>
            new(chatClient,
                $"You are a translation assistant who only responds in {targetLanguage}. Respond to any " +
                $"input by outputting the name of the input language and then translating the input to {targetLanguage}.");


        public async Task<string> AskWorkflowQuestionAsync(string prompt)
        {
            await using StreamingRun run = await InProcessExecution.StreamAsync(_workflow, prompt);

            await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

            List<Microsoft.Extensions.AI.ChatMessage> result = new();
            StringBuilder stringBuilder = new();
            await foreach (WorkflowEvent evt in run.WatchStreamAsync().ConfigureAwait(false))
            {
                if (evt is AgentRunUpdateEvent e)
                {
                    stringBuilder.Append($"{e.ExecutorId}: {e.Data}<br>");
                }
                else if (evt is WorkflowOutputEvent completed)
                {
                    result = (List<Microsoft.Extensions.AI.ChatMessage>)completed.Data!;
                    break;
                }
            }
            stringBuilder.Append("<br>Final output from the workflow:<br>");

            foreach (var message in result)
            {
                stringBuilder.Append($"{message.Role}: {message.Contents.FirstOrDefault()}<br>");
            }

            return stringBuilder.ToString();
        }
    }
}
