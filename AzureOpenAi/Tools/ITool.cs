using OpenAI.Assistants;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Tools;

public interface ITool
{
    public static abstract string Name { get; }

    public static abstract FunctionToolDefinition Definition { get; }
}