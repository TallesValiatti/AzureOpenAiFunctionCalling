using System.ClientModel;
using AzureOpenAiFunctionCalling.AzureOpenAi.Models;
using OpenAI;
using OpenAI.Assistants;

namespace AzureOpenAiFunctionCalling.AzureOpenAi.Services.AssistantService;

#pragma warning disable OPENAI001
public partial class AssistantService
{
    public async Task<string> RunAsync(RunRequest request)
    {
        var client = GetClient();
        var assistantClient = client.GetAssistantClient();

        var thread = (await assistantClient.GetThreadAsync(request.ThreadId)).Value;
        var assistant = (await assistantClient.GetAssistantAsync(request.AssistantId)).Value;
        
        await assistantClient.CreateMessageAsync(thread.Id, [MessageContent.FromText(request.Message)]);
        
        ThreadRun threadRun = await assistantClient.CreateRunAsync(thread.Id, assistant.Id);
        
        do
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            threadRun = await assistantClient.GetRunAsync(thread.Id, threadRun.Id);
            
            if (threadRun.Status == RunStatus.RequiresAction)
            {
                List<ToolOutput> toolOutputs = new();
                foreach (var requiredAction in threadRun.RequiredActions)
                {
                    var result = toolHandler.Handle(requiredAction);
                    
                    toolOutputs.Add(result);
                }
                
                threadRun = await assistantClient.SubmitToolOutputsToRunAsync(threadRun, toolOutputs);
            }
        }
        while (threadRun.Status == RunStatus.Queued || threadRun.Status == RunStatus.InProgress);
        
        AsyncPageableCollection<ThreadMessage> messagePage = assistantClient.GetMessagesAsync(thread.Id, ListOrder.NewestFirst);
        await using var enumerator = messagePage.GetAsyncEnumerator();
        var messageItem = await enumerator.MoveNextAsync() ? enumerator.Current : null;

        return messageItem?.Content.FirstOrDefault()?.Text ?? string.Empty;
    }
}
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.