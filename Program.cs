#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using CallSummarizerDemo.Services;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<WhisperConfiguration>(builder.Configuration.GetSection("Whisper"));
//builder.Services.AddSingleton<CROPValidation>();
//builder.Services.AddSingleton<CROPScoringService>();
//builder.Services.AddScoped<ICROPService, CROPService>();
builder.Services.AddSingleton<ISpeechToTextService, WhisperCppService>();
builder.Services.AddScoped<ISemanticKernelController, SemanticKernelController>();


builder.Services.AddSingleton<IChatCompletionService>(sp =>
{
    var kernel = sp.GetRequiredService<Kernel>();
    return kernel.GetRequiredService<IChatCompletionService>();
});

// Configure HttpClient with a higher timeout
builder.Services.AddHttpClient("OllamaClient")
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromMinutes(10);
    });

// // Configure and build the kernel
// builder.Services.AddSingleton(sp =>
// {
//     var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
//     var client = httpClientFactory.CreateClient("OllamaClient");

//     var kernelBuilder = Kernel.CreateBuilder()
//     .AddOllamaChatCompletion
//         (
//             modelId: "storm16k:latest",
//             endpoint: new Uri("http://localhost:11434")
//             //: new Uri("http://localhost:11434")
//         );

//     kernelBuilder.AddOllamaTextGeneration
//         (
//             modelId: "storm16k:latest",
//             endpoint: new Uri("http://localhost:11434")
//         );

//     var kernel = kernelBuilder.Build();

//     return kernel;
// });

builder.Services.AddSingleton(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var client = httpClientFactory.CreateClient("OllamaClient");

    var kernelBuilder = Kernel.CreateBuilder()
        .AddOpenAIChatCompletion(
            //modelId: "llama3.1:8b", //context is really small on this. doesn't summarize or follow instructions well.
            //modelId:"SummarizerV1.1:8b-nemo-4k",
            //modelId:"qwen4b-8k:latest", //not working
            modelId:"storm16k:latest",
            //modelId:"llama3.1-12k:latest",
            apiKey: null,
            httpClient:client,
            //endpoint: new Uri("http://100.127.244.111:11434/v1"));
            endpoint: new Uri("http://localhost:11434/v1"));

    var kernel = kernelBuilder.Build();

    return kernel;
});

// Configure Kestrel server options
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(5);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
