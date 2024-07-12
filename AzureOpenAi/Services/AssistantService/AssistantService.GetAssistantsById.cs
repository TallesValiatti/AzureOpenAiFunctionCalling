using System.ClientModel;
using AzureOpenAiFunctionCalling.AzureOpenAi.Models;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.AssistantService;

#pragma warning disable OPENAI001
public partial class AssistantService
{
    public async Task<AssistantResponse?> GetAssistantsByIdAsync(string id)
    {
        try
        {
            var client = GetClient();
            var assistantClient = client.GetAssistantClient();

            var assistant = await assistantClient.GetAssistantAsync(id);

            return new AssistantResponse(
                assistant.Value.Id, 
                assistant.Value.Name,
                assistant.Value.Instructions);
        }
        catch (ClientResultException ex) when (ex.Status == 404)
        {
            return null;
        }
    }
}
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.