﻿#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0070
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using CallSummarizerDemo.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI.Chat;
using System.Text.Json;

namespace CallSummarizerDemo.Services;

public interface ISemanticKernelController
{
    Task<string> GetSummary(string inText);
}

public class SemanticKernelController : ISemanticKernelController
{
    private bool Analysis = true;
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;

    public SemanticKernelController(Kernel kernel, IChatCompletionService chatCompletionService)
    {
        _kernel = kernel;
        _chatCompletionService = chatCompletionService;
    }

    private static string GetConversationPrompt()
    {
       // var examples = File.ReadAllText("StaticDemoMocks/TrainingExamples.json");

        //var exampleItems = JsonSerializer.Serialize(examples, new JsonSerializerOptions { WriteIndented = true });

        return @"You are a helpful assistant that analyzes transcribed conversations and responds only in the form of complete, valid JSON.
            If you do not find an item that corresponds to a parameter, please leave that parameter as null.
            For example, if you do not find a key point, you should include the parameter in the JSON object, but leave the ""keyPoints"" array, itself, as null.
            You should NEVER include any text in your response that is not part of the conversation in order to populate the JSON object. It is better to leave an item as null than to include text that did not exist in the conversation.

            Your response must be complete and valid JSON. Do not include any other text outside the JSON object.
            
            The following is a series of examples including transcripts followed by a possible correct JSON conversation analysis object:
            ";
    }

    public async Task<string> GetSummary(string inText)
    {

        var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = null,
            Temperature = 0.1,
            ChatSystemPrompt = GetConversationPrompt(),
            TopP = 0.8,
            //ChatSystemPrompt = "You are a helpful assistant that analyzes conversations between southern farm bureau life insurance employees and customers.",
            //ResponseFormat = "json_object"
            ResponseFormat = typeof(ConversationAnalysis)
        };

        ChatHistory chat = new();
        chat.AddMessage(AuthorRole.User, inText);

        var bleh = await _kernel.InvokePromptAsync(inText, new(openAIPromptExecutionSettings));

        var bleh2 =  JsonSerializer.Deserialize<ConversationAnalysis>(bleh.ToString())!;

        bleh2.Crop = await GetCROPFromAnalysis(bleh2);

        return JsonSerializer.Serialize(bleh2, new JsonSerializerOptions { WriteIndented = true });


        // Microsoft.SemanticKernel.ChatMessageContent result = await _chatCompletionService.GetChatMessageContentAsync(
        //    chatHistory: chat,
        //    executionSettings: openAIPromptExecutionSettings,
        //    kernel: _kernel);

        // if (Analysis)
        // {
        //     try
        //     {
        //         ConversationAnalysis analysis = JsonSerializer.Deserialize<ConversationAnalysis>(
        //         result.Content.ToString(),
        //         new JsonSerializerOptions 
        //         { 
        //             PropertyNameCaseInsensitive = true,
        //             AllowTrailingCommas = true,
        //             ReadCommentHandling = JsonCommentHandling.Skip
        //         });

        //         if (analysis != null)
        //         {
        //             analysis.Crop = await GetCROPFromAnalysis(analysis);
        //         }

        //         return JsonSerializer.Serialize(analysis, new JsonSerializerOptions 
        //         { 
        //             WriteIndented = true 
        //         });
        //     }
        //     catch (Exception e)
        //     {
        //         return "Error: " + e.Message;
        //     }
        // }

        // return result.Content?.ToString() ?? "Error: No content returned from analysis.";
    }

    public async Task<CROP> GetCROPFromAnalysis(ConversationAnalysis inText)
    {
        var examples = File.ReadAllText("StaticDemoMocks/TrainingExamples.json");

        var exampleItems = JsonSerializer.Serialize(examples, new JsonSerializerOptions { WriteIndented = true });

        var settings = new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = null,
            Temperature = 0.1,
            ChatSystemPrompt = CROPTemplate.Template + exampleItems,
            ResponseFormat = typeof(CROP)
            // ResponseFormat = GetCROPResponseFormat()
            //ResponseFormat = "json_object"
        };

        var stringy = JsonSerializer.Serialize(inText);

        ChatHistory chat = new();
        chat.AddMessage(AuthorRole.User, $"Analyze this conversation summary and identify any CROPs: {stringy}");

        Microsoft.SemanticKernel.ChatMessageContent result = await _chatCompletionService.GetChatMessageContentAsync(
            chatHistory: chat,
            executionSettings: settings,
            kernel: _kernel);

        if (!string.IsNullOrEmpty(result.Content))
        {
            try
            {
                var crop = JsonSerializer.Deserialize<CROP>(result.Content.ToString());
                if (crop != null)
                {
                    crop.OriginalAnalysis = inText.Id;
                }
                return crop;
            }
            catch (Exception)
            {
                return null;
            }
        }

        return null;
    }
}
