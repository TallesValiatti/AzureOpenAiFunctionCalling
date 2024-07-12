using AzureOpenAiFunctionCalling.AzureOpenAi.Models;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.AssistantService;

#pragma warning disable OPENAI001
public partial class AssistantService
{
    public async Task<ThreadResponse> CreateThreadAsync()
    {
        var client = GetClient();
        var assistantClient = client.GetAssistantClient();
        
        var thread = await assistantClient.CreateThreadAsync();

        return new ThreadResponse(thread.Value.Id);
    }
}
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.