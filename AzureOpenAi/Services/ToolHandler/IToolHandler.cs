using OpenAI.Assistants;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.ToolHandler;

public interface IToolHandler
{
    ToolOutput Handle(RequiredAction requiredAction);
}