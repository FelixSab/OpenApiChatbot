
using System.ComponentModel.DataAnnotations;

namespace OpenApiChatbot.Contracts.DTOs;

public record ChatCompletionRequestDto(

    [Required]
    string? UserMessage,

    bool IgnoreChatHistory,
    bool IsAdminChat,
    bool IsTraceLogEnabled
);
