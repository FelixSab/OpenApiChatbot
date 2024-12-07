using OpenApiChatbot.Contracts.DTOs;

namespace OpenApiChatbot.Contracts.Interfaces;

public interface IChatbotClient
{
    public bool BotExists(string? botId) => false;

    public ChatCreateResponseDto CreateChat(string botId);

    bool ChatExists(string chatId);

    ChatCompletionResponseDto PostChatCompletion(string chatId, ChatCompletionRequestDto request);
}
