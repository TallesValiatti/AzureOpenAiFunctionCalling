using System.ClientModel;
using AzureOpenAiFunctionCalling.AzureOpenAi.Models;
using AzureOpenAiFunctionCalling.AzureOpenAi.Tools;
using OpenAI.Assistants;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.AssistantService;

#pragma warning disable OPENAI001
public partial class AssistantService
{
    public async Task<AssistantResponse> CreateAssistantAsync(CreateAssistantRequest request)
    {
        var client = GetClient();
        var assistantClient = client.GetAssistantClient();
        
        var assistantOptions = new AssistantCreationOptions
        {
            Name = request.Name,
            Instructions = request.Description,
            ToolResources = new(),
            Tools =
            {
                EmailTool.Definition,
                BlogArticleWriter.Definition
            },
        };

        Assistant assistant = await assistantClient.CreateAssistantAsync(_azureOpenAiConfig.Model, assistantOptions);

        return new AssistantResponse(
            assistant.Id, 
            assistant.Name,
            assistant.Instructions);
    }
}
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.