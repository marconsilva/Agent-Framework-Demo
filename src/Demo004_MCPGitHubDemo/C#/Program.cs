using System.ClientModel;
using Microsoft.Agents.AI;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using ModelContextProtocol.Client;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

// Load user secrets from the project
var config = new ConfigurationBuilder().Build();

// Populate values from your OpenAI deployment
var modelId = config["AzureOpenAI:ModelId"] ?? "gpt-4o-demo";
var endpoint = config["AzureOpenAI:Endpoint"] ?? "https://{your-custom-endpoint}.openai.azure.com/";
var gitHubToken = config["Github:PersonalAccessToken"] ?? "{your-github-access-token}";



// Create an MCPClient for the GitHub server
await using var mcpClient = await McpClient.CreateAsync(new StdioClientTransport(new()
{
    Name = "MCPServer",
    Command = "podman",
    Arguments = ["run", "-i", "--rm", "-e", "GITHUB_PERSONAL_ACCESS_TOKEN", "ghcr.io/github/github-mcp-server"],
    EnvironmentVariables = new Dictionary<string, string?>() { { "GITHUB_PERSONAL_ACCESS_TOKEN", gitHubToken } },
}));


// Retrieve the list of tools available on the GitHub server
var mcpTools = await mcpClient.ListToolsAsync().ConfigureAwait(false);

AIAgent agent = new AzureOpenAIClient(
    new Uri(endpoint),
    new DefaultAzureCredential())
     .GetChatClient(modelId)
     .CreateAIAgent(
        instructions: "You answer questions related to GitHub repositories only.",
        tools: [.. mcpTools.Cast<AITool>()]);

// Invoke the agent and output the text result.
Console.WriteLine("GitHub MCP Agent Demo. Type a message and press Enter to send. Press Enter on an empty line to exit.\n");
Console.WriteLine("\tSummarize the last four commits to the microsoft/agent-framework repository?");
string? userInput;
do
{
    // Collect user input
    Console.Write("User > ");
    userInput = Console.ReadLine();

    if (string.IsNullOrEmpty(userInput))
        break;

    UserChatMessage chatMessage = new(userInput);

    // Invoke the agent and output the text result.
    ChatCompletion chatCompletion = await agent.RunAsync([userInput]);
    
    // Print the results
    Console.WriteLine("GitHub MCP Agent > " + chatCompletion.Content.Last().Text);

} while (userInput is not null);