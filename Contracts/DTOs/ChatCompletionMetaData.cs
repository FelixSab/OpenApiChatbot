
namespace OpenApiChatbot.Contracts.DTOs;

public record ChatCompletionMetaData(
    string[]? Tags,
    InformationSource[]? Sources
);
