using ai_apps_01.models;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace ai_apps_01.services;

public class AiService : IAiService
{
    private readonly IChatClient _chatClient;

    public AiService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task RunFirstSampel()
    {
        List<ChatMessage> messages = new List<ChatMessage>();

        messages.Add(new ChatMessage(ChatRole.System,
            "As AI assirant, you need to answer each of my questions with the langugae asked. If the question is in english, answer in English only. If the question is in Deutsch, answer in Deautsch only. Don't add extra text not related to the question being answered."));

        messages.Add(new ChatMessage(ChatRole.User, "My name is Adib. Ich bin 55 jahre alt."));

        messages.Add(new ChatMessage(ChatRole.User, "What's my name?"));
        var response2 = await _chatClient.GetResponseAsync(messages);

        messages.Add(new ChatMessage(ChatRole.Assistant, response2.Text));

        messages.Add(new ChatMessage(ChatRole.User, "Wie alt bin ich?"));
        response2 = await _chatClient.GetResponseAsync(messages);

        messages.Add(new ChatMessage(ChatRole.Assistant, response2.Text));

        messages.ForEach(message => { Console.WriteLine(message.Role + ":" + message.Text); });
    }

    public async Task SimpleChatApp()
    {
        var conversation = new List<ChatMessage>();

        conversation.Add(new ChatMessage(ChatRole.System,
            "Your are a smart agent. You are are going to have a chat with with the user. The topic of the chat is not identified before. So you need to change your tune and professionalism as per the topic that will be discussed with the user. Your answer shouldn't be long. You just provide direct answer to the user question without extra details. You should consider the chat history while answering the user's question. If the user asked a question that you have already answered, then no need to answer it again. Tell him that your question has already been answered before."));

        Console.WriteLine("This is chat app with a general agent. Start asking your question...");
        Console.WriteLine("To exist the chat, press enter without any text..");

        while (true)
        {
            var userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
                break;

            conversation.Add(new ChatMessage(ChatRole.User, userInput));

            var aiResponse = await _chatClient.GetResponseAsync(conversation);

            conversation.Add(new ChatMessage(ChatRole.Assistant, aiResponse.Text));

            Console.WriteLine($" ==> {aiResponse.Text}");
            Console.WriteLine($" ---------- ");
        }
    }

    public async Task SimpleChatAppWithStreamiing()
    {
        var conversation = new List<ChatMessage>();

        conversation.Add(new ChatMessage(ChatRole.System,
            "Your are a smart agent. You are are going to have a chat with with the user. The topic of the chat is not identified before. So you need to change your tune and professionalism as per the topic that will be discussed with the user. Your answer shouldn't be long. You just provide direct answer to the user question without extra details. You should consider the chat history while answering the user's question. If the user asked a question that you have already answered, then no need to answer it again. Tell him that your question has already been answered before."));

        Console.WriteLine("This is chat app with a general agent. Start asking your question...");
        Console.WriteLine("To exist the chat, press enter without any text..");

        List<ChatMessage> chatHistory = [];

        while (true)
        {
            var userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
                break;

            chatHistory.Add(new(ChatRole.User, Console.ReadLine()));

            List<ChatResponseUpdate> updates = [];
            await foreach (ChatResponseUpdate update in
                           _chatClient.GetStreamingResponseAsync(chatHistory))
            {
                Console.Write(update.Text);
                updates.Add(update);
            }

            Console.WriteLine();

            // Add the streamed response to history
            chatHistory.AddMessages(updates);
        }
    }

    public async Task SimpleStracturedOutput()
    {
        var review = "this is a good looking but with bad quality product";

        // The GetResponseAsync<T> extension method instructs the model to return a response matching your type, and automatically deserializes it.
        var response = await _chatClient.GetResponseAsync<Sentiment>($"What is the Sentiment of this review? {review}");

        Console.WriteLine($"Sentiment: {response.Result}");
    }

    public async Task SimpleStracturedOutputWithMultipleItems()
    {
        string[] reviews = [
            "Best purchase ever!",
            "Returned it immediately.",
            "Hello",
            "It works as advertised.",
            "The packaging was damaged but otherwise okay."
        ];

        foreach (var review in reviews)
        {
            var response = await _chatClient.GetResponseAsync<Sentiment>($"What is the Sentiment of this review? {review}");
            Console.WriteLine($"Review: {review} | Sentiment: {response.Result}");

        }
    }

    public async Task ComplexStracturedOutput()
    {
        var review = "This is bullshit product";
        var response = await _chatClient.GetResponseAsync<SentimentAnalysis>($"Analyze this review: {review}");
        
        Console.WriteLine($"Text: {response.Result.ResponseText}");
        Console.WriteLine($"Sentiment: {response.Result.ReviewSentiment}");
        Console.WriteLine($"Confidence: {response.Result.ConfidenceScore}");
        Console.WriteLine($"Key phrases: {string.Join(", ", response.Result.KeyPhrases)}");
    }

    public async Task ActionItemSample()
    {
        var meetingNotes = @"
    Discussed Q1 roadmap. John will prepare the budget by Friday (high priority).
    Sarah needs to review the marketing plan next week (medium priority).
    Team should update documentation ongoing (low priority).
";
        
        var response  = await _chatClient.GetResponseAsync<List<ActionItem>>($"Extract all the action items from the following meeting note: {meetingNotes}");
        
        response.Result?.ForEach( actionItem =>
        {
            Console.WriteLine($" - {actionItem.Task} | {actionItem.Assignee} | {actionItem.Priority}");
        });
    }
}