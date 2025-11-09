using System.ClientModel;
using Microsoft.Agents.AI;
using Azure.AI.OpenAI;
using System;
using Azure.Identity;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Configuration;

namespace JokerAgentDemo.Web.Components.Controllers
{
    public class JokerAgentContoller
    {
        private readonly ILogger<JokerAgentContoller> _logger;
        private readonly AIAgent _agent;

        public JokerAgentContoller(ILogger<JokerAgentContoller> logger, IConfiguration config)
        {
            _logger = logger;

            string endpoint = config["AzureOpenAI:Endpoint"] ?? throw new NullReferenceException("AzureOpenAI:Endpoint is null");
            string modelId = config["AzureOpenAI:ModelId"] ?? throw new NullReferenceException("AzureOpenAI:ModelId is null");

            _logger.LogInformation("Initializing JokerAgentContoller with Endpoint: {Endpoint} and ModelId: {ModelId}", endpoint, modelId);

            _agent = new AzureOpenAIClient(
                new Uri(endpoint),
                new DefaultAzureCredential())
                .GetChatClient(modelId)
                .CreateAIAgent(instructions: "You are good at telling jokes.", name: "Joker");
        }


        public async Task<string> GetJokeAsync(string prompt)
        {
            _logger.LogInformation("GetJokeAsync called with prompt: {Prompt}", prompt);

            UserChatMessage chatMessage = new(prompt);

            ChatCompletion chatCompletion = await _agent.RunAsync([chatMessage]);

            _logger.LogInformation("GetJokeAsync received response: {Response}", chatCompletion.Content.Last().Text);

            return chatCompletion.Content.Last().Text;
        }

    }
}
