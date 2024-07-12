using AzureOpenAiFunctionCalling.AzureOpenAi.Models;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.AssistantService;

public interface IAssistantService
{
    Task<IList<AssistantResponse>> ListAssistantAsync();
    Task<AssistantResponse?> GetAssistantsByIdAsync(string id);
    Task<AssistantResponse> CreateAssistantAsync(CreateAssistantRequest request);
    Task<ThreadResponse> CreateThreadAsync();
    Task<string> RunAsync(RunRequest request);
}