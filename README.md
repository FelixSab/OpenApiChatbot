# OpenApiChatbot Project

This project provides a modular and reusable .NET API solution to interact with a chatbot interface defined by an OpenAPI specification. It includes:

- **Contracts:** Data Transfer Objects (DTOs) and interfaces that define the data structures and client abstractions
- **API Layer:** Controllers and endpoints for creating chats and submitting chat completions

## Getting Started

### Prerequisites
- Docker
- Git

### Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/FelixSab/OpenApiChatbot.git
   cd OpenApiChatbot
   ```

2. **Start the Container**:
   ```bash
   docker-compose up --build --force-recreate
   ```
   After startup, access Swagger at `http://localhost:8080/swagger/index.html`

## OpenAPI Specification

The API exposes its OpenAPI Specification at `/openapi/v1.json`. During development, a Swagger endpoint is available at `/swagger/index.html`. The specification is generated during application startup from the attributes attached to controllers and endpoints:

```csharp
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
    // ...
}
```

## Validation

The application implements validation at two levels: Controller and Model.

### Controller Validation
Controller validation occurs within the endpoint and handles business logic:
```csharp
if (!_chatbotClient.BotExists(request.BotId))
{
    return NotFound(CreateNotFound($"No bot found with the provided botId: {request.BotId}"));
}
```

### Model Validation
Model validation uses Data Annotations and is handled by ASP.NET Core:
```csharp
public record ChatCreateRequestDto(
    [Required]
    string? BotId
);
```

To maintain consistent error responses, the application transforms validation failures into ProblemDetails responses:
```csharp
public static IServiceCollection ConfigureModelValidation(this IServiceCollection services)
{
    return services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .SelectMany(ms => ms.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

            var detailMessage = string.Join("; ", errors);

            var problemDetails = new ProblemDetails
            {
                Title = "Invalid input",
                Detail = detailMessage,
                Status = StatusCodes.Status400BadRequest
            };

            return new BadRequestObjectResult(problemDetails);
        };
    });
}
```

## Error Handling

The application throws exceptions only for unexpected behavior, which always produces a 500 Internal Server Error. This is handled by ASP.NET Core's exception handler middleware:

```csharp
app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext httpContext) =>
{
    var problemDetails = new ProblemDetails
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "An unexpected error occurred",
        Detail = "Please try again later or contact support if the problem persists."
    };

    return Results.Problem(problemDetails);
});
```

## Health Checks

The application implements ASP.NET Core's health check system for monitoring application health:

```csharp
// Service configuration
builder.Services.AddHealthChecks();

// Endpoint mapping
app.MapHealthChecks("/health");
```

This endpoint returns a 200 OK status when the application is healthy and can be used by container orchestrators and monitoring systems to verify the application's status.

## Logging

HTTP logging is enabled through the service builder:
```csharp
builder.Services.AddHttpLogging(o => { });
app.UseHttpLogging();
```

Log levels are configured in `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware": "Information"
    }
  }
}
```

## Chatbot Client

The Chatbot Client API is defined through an interface in the `Contracts` project, which serves as an intermediary between the REST API and the application:

```csharp
public interface IChatbotClient
{
    public bool BotExists(string? botId) => false;
    public ChatCreateResponseDto CreateChat(string botId);
    bool ChatExists(string chatId);
    ChatCompletionResponseDto PostChatCompletion(string chatId, ChatCompletionRequestDto request);
}
```

For development purposes, a mock implementation is provided and registered as a service:
```csharp
public static class ChatbotClientExtensions
{
    public static IServiceCollection AddChatbotClient(this IServiceCollection services)
    {
        return services.AddTransient<IChatbotClient, MockChatbotClient>();
    }
}
```

## Future Improvements

The following features could enhance the application:

1. **API Versioning**
   - Add Microsoft.AspNetCore.Mvc.Versioning for API evolution

2. **Authentication/Authorization**
   - Implement JWT authentication or API keys
   - Add role-based access control

3. **Rate Limiting**
   - Add middleware to prevent API abuse

4. **Caching**
   - Implement response caching
   - Add distributed cache support

5. **Configuration Management**
   - Add environment-specific configurations
   - Implement user secrets for development

6. **Testing**
   - Add unit tests
   - Implement integration tests
   - Add load testing

7. **Monitoring**
   - Integrate Application Insights
   - Add performance metrics collection

8. **CI/CD**
   - Set up automated build pipeline
   - Configure deployment automation

10. **Background Jobs**
    - Add job queue system
    - Implement async operation handling
