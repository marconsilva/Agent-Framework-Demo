dotnet user-secrets init
dotnet user-secrets set "AzureOpenAI:Endpoint" $AZURE_OPENAI_ENDPOINT
dotnet user-secrets set "AzureOpenAI:ModelId" $AZURE_OPENAI_MODEL_ID
dotnet user-secrets set "GitHub:PersonalAccessToken" $GITHUB_PERSONAL_ACCESS_TOKEN
dotnet restore
dotnet build
