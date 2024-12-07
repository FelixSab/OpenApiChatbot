using Microsoft.AspNetCore.Mvc;
using OpenApiChatbot.API.Attributes;
using OpenApiChatbot.Contracts.DTOs;
using OpenApiChatbot.Contracts.Interfaces;
using static OpenApiChatbot.API.Utils.ErrorResponses;

namespace OpenApiChatbot.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController(IChatbotClient chatbotClient) : Controller
{
    private readonly IChatbotClient _chatbotClient = chatbotClient ?? throw new ArgumentNullException(nameof(chatbotClient));

    [HttpPost]
    [ProducesResponseType(typeof(ChatCreateResponseDto), StatusCodes.Status200OK)]
    [ProducesProblemDetails(StatusCodes.Status400BadRequest)]
    [ProducesProblemDetails(StatusCodes.Status404NotFound)]
    public ActionResult<ChatCreateResponseDto> CreateChat([FromBody] ChatCreateRequestDto request)
    {
        if (!_chatbotClient.BotExists(request.BotId))
        {
            return NotFound(CreateNotFound($"No bot found with the provided botId: {request.BotId}"));
        }

        var response = _chatbotClient.CreateChat(request.BotId! /* validated through data annotations */);

        return Ok(response);
    }

    [HttpPost("{chatId}/completions")]
    [ProducesResponseType(typeof(ChatCompletionResponseDto), StatusCodes.Status200OK)]
    [ProducesProblemDetails(StatusCodes.Status400BadRequest)]
    [ProducesProblemDetails(StatusCodes.Status404NotFound)]
    public ActionResult<ChatCompletionResponseDto> ChatCompletion(
        [FromRoute] string chatId,
        [FromBody] ChatCompletionRequestDto request)
    {
        if (!_chatbotClient.ChatExists(chatId))
        {
            return NotFound(CreateNotFound($"No chat found with the provided chatId: {chatId}"));
        }

        var response = _chatbotClient.PostChatCompletion(chatId, request);

        return Ok(response);
    }
}
