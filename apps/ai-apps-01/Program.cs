using ai_apps_01.services;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaSharp;

namespace ai_apps_01;

class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<IChatClient>(_ =>
            new OllamaApiClient(
                new Uri("http://localhost:11434"),
                "llama3.2"));

        builder.Services.AddScoped<IAiService, AiService>();
        using var host = builder.Build();

        var aiService = host.Services.GetRequiredService<IAiService>();


        // aiService.RunFirstSampel().Wait();
        //  aiService.SimpleChatApp().Wait();
        // aiService.SimpleChatAppWithStreamiing().Wait();
       // aiService.SimpleStracturedOutput().Wait();
       // aiService.SimpleStracturedOutputWithMultipleItems().Wait();
       // aiService.ComplexStracturedOutput().Wait();
       // aiService.ActionItemSample().Wait();
       aiService.FunctionSample().Wait();
    }
}