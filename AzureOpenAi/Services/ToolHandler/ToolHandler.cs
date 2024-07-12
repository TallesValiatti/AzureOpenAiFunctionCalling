using System.Text.Json;
using AzureOpenAiFunctionCalling.AzureOpenAi.Services.ChatCompletions;
using AzureOpenAiFunctionCalling.AzureOpenAi.Tools;
using OpenAI.Assistants;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.ToolHandler;

public class ToolHandler(ILogger<ToolHandler> logger, IChatCompletionsService chatCompletionsService) : IToolHandler
{
    public ToolOutput Handle(RequiredAction requiredAction)
    {
        using JsonDocument argumentsJson = JsonDocument.Parse(requiredAction.FunctionArguments);

        if (requiredAction.FunctionName == EmailTool.Name)
        {
            return HandlerEmailTool(requiredAction, argumentsJson);
        }
        
        if (requiredAction.FunctionName == BlogArticleWriter.Name)
        {
            return HandlerBlogArticleWriterTool(requiredAction, argumentsJson);
        }
        
        return null!;
    }

    private ToolOutput HandlerBlogArticleWriterTool(RequiredAction requiredAction, JsonDocument argumentsJson)
    {
        string title = argumentsJson.RootElement.GetProperty("title").GetString()!;
        string generalIdea = argumentsJson.RootElement.GetProperty("generalIdea").GetString()!;
        int numberOfWords = argumentsJson.RootElement.GetProperty("numberOfWords").GetInt32()!;

        var result = new BlogArticleWriter().Execute(
            logger, 
            chatCompletionsService, 
            title, 
            generalIdea, 
            numberOfWords);
        
        return new ToolOutput(requiredAction.ToolCallId, result);
    }

    private ToolOutput HandlerEmailTool(RequiredAction requiredAction, JsonDocument argumentsJson)
    {
        string receiverEmail = argumentsJson.RootElement.GetProperty("receiverEmail").GetString()!;
        string subject = argumentsJson.RootElement.GetProperty("subject").GetString()!;
        string message = argumentsJson.RootElement.GetProperty("message").GetString()!;

        var result = new EmailTool().Execute(
            logger, 
            receiverEmail, 
            subject,
            message);
        
        return new ToolOutput(requiredAction.ToolCallId, result);
    }
}