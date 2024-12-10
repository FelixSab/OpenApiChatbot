using OpenApiChatbot.Contracts.DTOs;
using OpenApiChatbot.Contracts.Interfaces;

namespace OpenApiChatbot.API;

// This chatbot client would usually be declared in another assembly,
// which is consumed by the API, but for simplicity purposes I put it here
public class MockChatbotClient : IChatbotClient
{
    public bool BotExists(string? botId)
    {
        return botId == "valid-bot-id";
    }

    public bool ChatExists(string chatId)
    {
        return chatId == "valid-chat-id";
    }

    public ChatCreateResponseDto CreateChat(string botId)
    {
        return new ChatCreateResponseDto(
            ChatId: Guid.NewGuid().ToString(),
            BotId: botId,
            BotDisplayName: "Sample Bot",
            BotDisplayMessage: "Hello, I am your assistant.",
            BotDescription: "I can help you with various questions.",
            BotSampleQuestion1: "How can I assist you today?",
            BotSampleQuestion2: "What would you like to learn?",
            IsUserSystemMessageSupported: true
        );
    }

    public ChatCompletionResponseDto PostChatCompletion(string chatId, ChatCompletionRequestDto request)
    {
        var response = new ChatCompletionResponseDto(
            CompletionId: Guid.NewGuid().ToString(),
            ChatId: chatId,
            UserMessage: request.UserMessage,
            AssistantMessage: "This is a mock response from the assistant.",
            PromptTokens: 10,
            CompletionTokens: 5,
            TotalTokens: 15,
            BotId: "valid-bot-id",
            BotDisplayName: "Sample Bot",
            BotDisplayMessage: "Hello, I am your assistant.",
            BotDescription: "I can help you with various questions.",
            ChatDisplayName: "Sample Chat",
            MetaData: new ChatCompletionMetaData(
                Tags: ["example", "test"],
                Sources:
                [
                    new InformationSource(
                        Id: "source1",
                        FileName: "sourcefile.txt",
                        Url: "http://example.com/source",
                        Language: "en",
                        CreateTicketUrl: null
                    )
                ]
            ),
            TraceLog: request.IsTraceLogEnabled ? "Trace log data here..." : null
        );

        return response;
    }
}


public static class ChatbotClientExtensions
{
    public static IServiceCollection AddChatbotClient(this IServiceCollection services)
    {
        return services.AddTransient<IChatbotClient, MockChatbotClient>();
    }
}
