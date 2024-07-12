namespace AzureOpenAiFunctionCalling.AzureOpenAi.Models;

public record RunRequest(string AssistantId, string ThreadId, string Message);