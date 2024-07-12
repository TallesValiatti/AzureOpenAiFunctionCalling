using System.ClientModel;
using AzureOpenAiFunctionCalling.AzureOpenAi.Models;
using OpenAI.Assistants;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.AssistantService;

#pragma warning disable OPENAI001
public partial class AssistantService
{
    public async Task<IList<AssistantResponse>> ListAssistantAsync()
    {
        var client = GetClient();
        var assistantClient = client.GetAssistantClient();

         AsyncPageableCollection<Assistant> assistants = assistantClient.GetAssistantsAsync();

         var result = new List<AssistantResponse>();
         await foreach (Assistant assistant in assistants)
         {
             result.Add(new AssistantResponse(
                 assistant.Id, 
                 assistant.Name,
                 assistant.Instructions));
         }

         return result;
    }
}
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.