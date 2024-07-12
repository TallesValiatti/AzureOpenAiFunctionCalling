using System.Text.Json;
using OpenAI.Assistants;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Tools;

public class EmailTool : ITool
{
    public static string Name => nameof(EmailTool);

    public static FunctionToolDefinition Definition => new(
        name: Name,
        description: "Send emails",
        parameters: BinaryData.FromObjectAsJson(
            new
            {
                Type = "object",
                Properties = new
                {
                    ReceiverEmail = new
                    {
                        Type = "string",
                        Description = "E-mail of the person who will receive the email",
                    },
                    Subject = new
                    {
                        Type = "string",
                        Description = "E-mail subject",
                    },
                    Message = new
                    {
                        Type = "string",
                        Description = "Email message",
                    },
                },
                Required = new[] { "ReceiverEmail", "Subject", "Message" },
            },
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

    public string Execute(ILogger logger, string receiverEmail, string subject, string message)
    {
        logger.LogInformation($"Email sent to '{receiverEmail}' with subject '{subject}' and message '{message}'");

        return "Email sent successfully";
    }
}