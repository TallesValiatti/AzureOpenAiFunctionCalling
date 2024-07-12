using AzureOpenAiFunctionCalling.AzureOpenAi;
using AzureOpenAiFunctionCalling.AzureOpenAi.Models;
using AzureOpenAiFunctionCalling.AzureOpenAi.Services.AssistantService;
using AzureOpenAiFunctionCalling.AzureOpenAi.Services.ChatCompletions;
using AzureOpenAiFunctionCalling.AzureOpenAi.Services.ToolHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureOpenAiConfig>(builder.Configuration.GetSection("AzureOpenAi"));
builder.Services.AddScoped<IAssistantService, AssistantService>();
builder.Services.AddScoped<IToolHandler, ToolHandler>();
builder.Services.AddScoped<IChatCompletionsService, ChatCompletionsService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/assistants", async (IAssistantService service) => 
        Results.Ok(await service.ListAssistantAsync()))
.WithName("ListAssistants")
.WithOpenApi();

app.MapGet("/assistants/{id}", async (string id, IAssistantService service) => 
        Results.Ok(await service.GetAssistantsByIdAsync(id)))
    .WithName("GetAssistantsById")
    .WithOpenApi();

app.MapPost("/assistants", async (CreateAssistantRequest request, IAssistantService service) => 
        Results.Ok(await service.CreateAssistantAsync(request)))
    .WithName("CreateAssistant")
    .WithOpenApi();

app.MapPost("/threads", async (IAssistantService service) => 
        Results.Ok(await service.CreateThreadAsync()))
    .WithName("CreateThread")
    .WithOpenApi();

app.MapPost("/run", async (RunRequest request, IAssistantService azureOpenApiService) => 
        Results.Ok(await azureOpenApiService.RunAsync(request)))
    .WithName("Run")
    .WithOpenApi();

app.Run();
