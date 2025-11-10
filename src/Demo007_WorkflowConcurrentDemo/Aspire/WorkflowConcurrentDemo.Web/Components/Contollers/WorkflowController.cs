
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
using WorkflowConcurrentDemo.Web.Components.Tools;

namespace WorkflowConcurrentDemo.Web.Components.Contollers
{
    public class WorkflowController
    {
        private readonly ILogger<WorkflowController> _logger;
        private readonly ChatClientAgent _physicist;
        private readonly ChatClientAgent _chemist;
        private readonly IChatClient _chatClient;
        private readonly Workflow _workflow;


        public WorkflowController(ILogger<WorkflowController> logger, IConfiguration config)
        {
            _logger = logger;

            string endpoint = config["AzureOpenAI:Endpoint"] ?? throw new NullReferenceException("AzureOpenAI:Endpoint is null");
            string modelId = config["AzureOpenAI:ModelId"] ?? throw new NullReferenceException("AzureOpenAI:ModelId is null");

            _logger.LogInformation("Initializing JokerAgentContoller with Endpoint: {Endpoint} and ModelId: {ModelId}", endpoint, modelId);

            _chatClient = new AzureOpenAIClient(
                new Uri(endpoint),
                new DefaultAzureCredential())
                .GetChatClient(modelId)
                .AsIChatClient();
            // Create the executors
            _physicist = new(
                _chatClient,
                name: "Physicist",
                instructions: "You are an expert in physics. You answer questions from a physics perspective."
            );
            _chemist = new(
                _chatClient,
                name: "Chemist",
                instructions: "You are an expert in chemistry. You answer questions from a chemistry perspective."
            );
            var startExecutor = new ConcurrentStartExecutor();
            var aggregationExecutor = new ConcurrentAggregationExecutor();

            _workflow = new WorkflowBuilder(startExecutor)
                .AddFanOutEdge(startExecutor, targets: [_physicist, _chemist])
                .AddFanInEdge(aggregationExecutor, sources: [_physicist, _chemist])
                .WithOutputFrom(aggregationExecutor)
                .Build();

        }

        public async Task<string> AskWorkflowQuestionAsync(string prompt)
        {
            await using StreamingRun run = await InProcessExecution.StreamAsync(_workflow, prompt);

            StringBuilder stringBuilder = new();
            await foreach (WorkflowEvent evt in run.WatchStreamAsync().ConfigureAwait(false))
            {
                if (evt is WorkflowOutputEvent output)
                {
                    
                    var htmloutput = output.Data.ToString().Replace("\n", "<br>");
                    stringBuilder.AppendLine($"Workflow completed with results:<br>{htmloutput}");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
