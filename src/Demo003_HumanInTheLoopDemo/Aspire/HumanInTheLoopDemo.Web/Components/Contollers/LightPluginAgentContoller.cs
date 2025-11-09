using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using HumanInTheLoopDemo.Web.Components.Plugins;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;

namespace HumanInTheLoopDemo.Web.Components.Contollers
{
    public class LightPluginAgentContoller
    {
        private readonly ILogger<LightPluginAgentContoller> _logger;
        private readonly AIAgent _agent;
        private readonly AgentThread _lightsAgentThread;

        public LightPluginAgentContoller(ILogger<LightPluginAgentContoller> logger, IConfiguration config, LightsPlugin lightsPlugin)
        {
            _logger = logger;

            string endpoint = config["AzureOpenAI:Endpoint"] ?? throw new NullReferenceException("AzureOpenAI:Endpoint is null");
            string modelId = config["AzureOpenAI:ModelId"] ?? throw new NullReferenceException("AzureOpenAI:ModelId is null");

            _logger.LogInformation("Initializing JokerAgentContoller with Endpoint: {Endpoint} and ModelId: {ModelId}", endpoint, modelId);

            _agent = new AzureOpenAIClient(
                new Uri(endpoint),
                new DefaultAzureCredential())
                .GetChatClient(modelId)
                .CreateAIAgent(
                    instructions: "You answer questions about lights and their states.",
                    name: "LightsAgent",
                    description: "An agent that answers questions about lights and can control their states.",
                    tools: [.. lightsPlugin.AsAITools()]
                );

            _lightsAgentThread = _agent.GetNewThread();
        }

        public async Task<LightPluginAgentResponse> AskLightAgentAsync(List<Microsoft.Extensions.AI.ChatMessage> approvalRequestMessages)
        {
            var response = await _agent.RunAsync(approvalRequestMessages, _lightsAgentThread);

            var userInputRequests = response.UserInputRequests.ToList();


            if (userInputRequests.Any())
            {                 
                _logger.LogInformation("AskLightAgentAsync requires user approval for: {Requests}", string.Join(", ", userInputRequests.OfType<FunctionApprovalRequestContent>().Select(r => r.FunctionCall.Name)));
                return new LightPluginAgentResponse()
                {
                    Response = string.Empty,
                    ResponseType = AgentResponseType.ApprovalRequest,
                    FunctionCallsToApprove = userInputRequests.OfType<FunctionApprovalRequestContent>().Select(functionCallApprovalRequest => functionCallApprovalRequest)
                };
            }
            else
            {
                _logger.LogInformation("AskLightAgentAsync received response: {Response}", response.ToString() ?? string.Empty);
                return new LightPluginAgentResponse()
                {
                    Response = response.ToString(),
                    ResponseType = AgentResponseType.Text,
                    FunctionCallsToApprove = Array.Empty<FunctionApprovalRequestContent>()
                };
            }

        }

        public async Task<LightPluginAgentResponse> AskLightAgentAsync(string prompt)
        {
            _logger.LogInformation("AskLightAgentAsync called with prompt: {Prompt}", prompt);

            UserChatMessage chatMessage = new(prompt);
            var response = await _agent.RunAsync(prompt, _lightsAgentThread);

            var userInputRequests = response.UserInputRequests.ToList();

            if (userInputRequests.Any())
            {
                _logger.LogInformation("AskLightAgentAsync requires user approval for: {Requests}", string.Join(", ", userInputRequests.OfType<FunctionApprovalRequestContent>().Select(r => r.FunctionCall.Name)));
                return new LightPluginAgentResponse()
                {
                    Response = string.Empty,
                    ResponseType = AgentResponseType.ApprovalRequest,
                    FunctionCallsToApprove = userInputRequests.OfType<FunctionApprovalRequestContent>().Select(functionCallApprovalRequest => functionCallApprovalRequest)
                };
            }
            else
            {
                _logger.LogInformation("AskLightAgentAsync received response: {Response}", response.ToString() ?? string.Empty);

                return new LightPluginAgentResponse()
                    {
                        Response = response.ToString(),
                        ResponseType = AgentResponseType.Text,
                        FunctionCallsToApprove = Array.Empty<FunctionApprovalRequestContent>()
                };
            }

        }

    }
}
