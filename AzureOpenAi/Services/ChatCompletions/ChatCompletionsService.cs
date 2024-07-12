using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.ChatCompletions;

public class ChatCompletionsService(IOptions<AzureOpenAiConfig> config) : IChatCompletionsService
{
    private AzureOpenAIClient GetClient()
    {
        return new AzureOpenAIClient(
            new Uri(config.Value.Endpoint), 
            new AzureKeyCredential(config.Value.Key));
    }
    
    public string Complete(string userMessage, string systemMessage)
    {
        var client = GetClient();
        var chatClient = client.GetChatClient(config.Value.Model);

        var msgs = new List<ChatMessage>();

        if (!string.IsNullOrWhiteSpace(systemMessage))
        {
            msgs.Add(new SystemChatMessage(systemMessage));
        }

        msgs.Add(new UserChatMessage(userMessage));
        
        var completion = chatClient.CompleteChat(msgs.ToArray());

        var result = completion.Value.Content[0].Text;

        return result;
    }

    public string Complete(string userMessage)
    {
        return Complete(userMessage, string.Empty);
    }
}