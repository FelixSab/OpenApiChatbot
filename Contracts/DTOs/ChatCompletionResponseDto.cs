

namespace OpenApiChatbot.Contracts.DTOs;

public record ChatCompletionResponseDto(
    string? CompletionId,
    string? ChatId,
    string? UserMessage,
    string? AssistantMessage,
    int PromptTokens,
    int CompletionTokens,
    int TotalTokens,
    string? BotId,
    string? BotDisplayName,
    string? BotDisplayMessage,
    string? BotDescription,
    string? ChatDisplayName,
    ChatCompletionMetaData MetaData,
    string? TraceLog
);
