using Azure;
using Azure.AI.OpenAI;
using AzureOpenAiFunctionCalling.AzureOpenAi.Services.ToolHandler;
using Microsoft.Extensions.Options;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.AssistantService;

public partial class AssistantService(IOptions<AzureOpenAiConfig> config, IToolHandler toolHandler) : IAssistantService
{
    private readonly AzureOpenAiConfig _azureOpenAiConfig = config.Value;
    
    private AzureOpenAIClient GetClient()
    {
        return new AzureOpenAIClient(
            new Uri(_azureOpenAiConfig.Endpoint), 
            new AzureKeyCredential(_azureOpenAiConfig.Key));
    }
}