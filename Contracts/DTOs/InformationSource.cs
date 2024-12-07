
namespace OpenApiChatbot.Contracts.DTOs;

public record InformationSource(
    string? Id,
    string? FileName,
    string? Url,
    string? Language,
    string? CreateTicketUrl
);