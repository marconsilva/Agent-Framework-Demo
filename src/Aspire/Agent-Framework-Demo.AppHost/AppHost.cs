using System.Data.Common;
using System.Reflection.Metadata;

var builder = DistributedApplication.CreateBuilder(args);

var azureOpenAIEndpointParameter = builder.AddParameter("AzureOpenAIEndpoint");
var azureOpenAIModelIdParameter = builder.AddParameter("AzureOpenAIModelId");

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Agent_Framework_Demo_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Agent_Framework_Demo_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.JokerAgentDemo_Web>("Demo001-JokerAgentDemo")
    .WithEnvironment("AzureOpenAI:Endpoint", azureOpenAIEndpointParameter)
    .WithEnvironment("AzureOpenAI:ModelId", azureOpenAIModelIdParameter)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.LightPluginDemo_Web>("Demo002-LightPluginDemo")
    .WithEnvironment("AzureOpenAI:Endpoint", azureOpenAIEndpointParameter)
    .WithEnvironment("AzureOpenAI:ModelId", azureOpenAIModelIdParameter)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);


builder.AddProject<Projects.HumanInTheLoopDemo_Web>("Demo003-HumanInTheLoopDemo")
    .WithEnvironment("AzureOpenAI:Endpoint", azureOpenAIEndpointParameter)
    .WithEnvironment("AzureOpenAI:ModelId", azureOpenAIModelIdParameter)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
