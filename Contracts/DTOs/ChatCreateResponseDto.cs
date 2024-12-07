namespace OpenApiChatbot.Contracts.DTOs;

public record ChatCreateResponseDto(
    string? ChatId,
    string? BotId,
    string? BotDisplayName,
    string? BotDisplayMessage,
    string? BotDescription,
    string? BotSampleQuestion1,
    string? BotSampleQuestion2,
    bool IsUserSystemMessageSupported
);
