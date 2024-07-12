using System.Text.Json;
using AzureOpenAiFunctionCalling.AzureOpenAi.Services.ChatCompletions;
using OpenAI.Assistants;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Tools;

public class BlogArticleWriter : ITool
{
    private const string MessageTemplate = """
         Create a article about '{generalIdea}' in {numberOfWords} words.
         Use the '{title}' as title.
         Outputs only the article 

         -----

         Use the following schema:

         <title>
         
         <content>
         """;
    
    public static string Name => nameof(BlogArticleWriter);

    public static FunctionToolDefinition Definition => new(
        name: Name,
        description: "Write article for a personal tech blog",
        parameters: BinaryData.FromObjectAsJson(
            new
            {
                Type = "object",
                Properties = new
                {
                    Title = new
                    {
                        Type = "string",
                        Description = "The title of the article",
                    },
                    GeneralIdea = new
                    {
                        Type = "string",
                        Description = "The idea of the article",
                    },
                    NumberOfWords = new
                    {
                        Type = "integer",
                        Description = "The number of words",
                    },
                },
                Required = new[] { "Title", "GeneralIdea", "NumberOfWords" },
            },
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

    public string Execute(
        ILogger logger, 
        IChatCompletionsService chatCompletionsService, 
        string title, 
        string generalIdea, 
        int numberOfWords)
    {
        var formattedMsg = MessageTemplate
            .Replace("{generalIdea}", generalIdea)
            .Replace("{title}", title)
            .Replace("{numberOfWords}", numberOfWords.ToString());
        
        var result = chatCompletionsService.Complete(formattedMsg);

        logger.LogInformation($"Article with title '{title}' and general idea '{generalIdea}' was created");

        return result;
    }
}