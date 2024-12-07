using System.ComponentModel.DataAnnotations;

namespace OpenApiChatbot.Contracts.DTOs;

public record ChatCreateRequestDto(

    [Required]
    string? BotId

);
