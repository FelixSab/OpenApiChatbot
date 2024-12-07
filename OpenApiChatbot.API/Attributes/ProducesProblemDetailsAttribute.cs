using Microsoft.AspNetCore.Mvc;

namespace OpenApiChatbot.API.Attributes;

public class ProducesProblemDetailsAttribute : ProducesResponseTypeAttribute<ProblemDetails>
{
    public ProducesProblemDetailsAttribute(int statusCode) : base(statusCode)
    {
    }

    public ProducesProblemDetailsAttribute(int statusCode, string contentType, params string[] additionalContentTypes) : base(statusCode, contentType, additionalContentTypes)
    {
    }
}
