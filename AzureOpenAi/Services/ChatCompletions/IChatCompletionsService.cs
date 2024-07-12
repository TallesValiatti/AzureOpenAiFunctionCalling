namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.ChatCompletions;

public interface IChatCompletionsService
{
    string Complete(string systemMessage, string userMessage);
    string Complete(string userMessage);
}