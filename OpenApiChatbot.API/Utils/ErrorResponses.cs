using Microsoft.AspNetCore.Mvc;

namespace OpenApiChatbot.API.Utils;

public static class ErrorResponses
{
    public static ProblemDetails CreateNotFound(string message) => new()
    {
        Title = "Not found",
        Detail = message,
        Status = StatusCodes.Status404NotFound
    };

    public static ProblemDetails CreateInvalidInput(string message) => new()
    {
        Title = "Invalid input",
        Detail = message,
        Status = StatusCodes.Status400BadRequest
    };
}
