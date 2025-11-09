using Azure.AI.OpenAI;
using Azure.Identity;
using LightPluginDemo.Web.Components.Plugins;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;

namespace LightPluginDemo.Web.Components.Contollers
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


        public async Task<string> AskLightAgentAsync(string prompt)
        {
            _logger.LogInformation("AskLightAgentAsync called with prompt: {Prompt}", prompt);

            UserChatMessage chatMessage = new(prompt);
            ChatCompletion chatCompletion = await _agent.RunAsync([chatMessage], _lightsAgentThread);

            _logger.LogInformation("AskLightAgentAsync received response: {Response}", chatCompletion.Content.Last().Text);

            return chatCompletion.Content.Last().Text;
        }

    }
}
