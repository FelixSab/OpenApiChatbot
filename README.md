# OpenApiChatbot Project

This project provides a modular and reusable .NET API solution to interact with a chatbot interface defined by an OpenAPI specification. It includes:

- **Contracts:** DTOs and interfaces that define the data structures and client abstractions.
- **API Layer:** Controllers and endpoints for creating chats and submitting chat completions.

## Getting Started

### Prerequisites
- Docker
- git

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
   Afterwards, you can access Swagger via `http://localhost:8080/swagger/index.html`

## OpenApi Specification

The API exposes it's OpenApi Specification on `/openapi/v1.json`. During development a Swagger Endpoint is exposed as well on `/swagger/index.html`. The Specification is generated during Startup of the application from the Attributes attached to our Controllers and endpoints:

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
      ...
```

## Validation

We have two ways to validate data. Within the Controller and within the Model.

1. Validation within the Controller happens within the called endpoint and may look like this:
   ```csharp
   if (!_chatbotClient.BotExists(request.BotId))
   {
      return NotFound(CreateNotFound($"No bot found with the provided botId: {request.BotId}"));
   }
   ```

2. Validation within the Model happens via Data Annotations, is handled by Asp.net Core internally and returns a Bad Request Result. Data Annotations look like this:
   ```csharp
   public record ChatCreateRequestDto(
      [Required]
      string? BotId
   );
   ```
   We want to always return a ProblemDetails Response, which has a Status, a Title and a Description, which is why we have to transform the Bad Request Response produced by Asp.net from failed Data Annotation validations:
   ```csharp
   public static IServiceCollection ConfigureModelValidation(this IServiceCollection services)
   {
      return services.Configure<ApiBehaviorOptions>(options =>
      {
         options.InvalidModelStateResponseFactory = context =>
         {
               // Extract validation error messages
               var errors = context.ModelState
                  .Where(ms => ms.Value?.Errors.Count > 0)
                  .SelectMany(ms => ms.Value!.Errors)
                  .Select(e => e.ErrorMessage)
                  .ToArray();

               // Combine all error messages into a single string, or handle them individually
               var detailMessage = string.Join("; ", errors);

               // Return a ProblemDetails object styled like your other error responses
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

Exceptions are only thrown when truly unexpected behaviour is occurring, these will always produce a 500 Internal Server Error. For that we are using an Exception Handler already implemented by Asp.net and map it to an endpoint which will generate a generic Internal Server Error:

```csharp
app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext httpContext) =>
{
    var problemDetails = new ProblemDetails
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "An unexpected error occurred!",
        Detail = "Please try again later or contact support if the problem persists."
    };

    return Results.Problem(problemDetails);
});
```

## Logging
We have enabled the http logging via our service builder:
```csharp
builder.Services.AddHttpLogging(o => { });

app.UseHttpLogging();
```
The Loglevel is configured by the appsettings.json:
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

The Chatbot Client Api is exposed via an interface in a seperate `Contracts` Project, which acts as an intermediary between the Rest API and the rest of the application. 
```csharp
public interface IChatbotClient
{
    public bool BotExists(string? botId) => false;

    public ChatCreateResponseDto CreateChat(string botId);

    bool ChatExists(string chatId);

    ChatCompletionResponseDto PostChatCompletion(string chatId, ChatCompletionRequestDto request);
}
```

For purposes of this case study, a mock implementation has been provided, which is registered as a Service, so it can be consumed by our Controllers:
```csharp
public static class ChatbotClientExtensions
{
    public static IServiceCollection AddChatbotClient(this IServiceCollection services)
    {
        return services.AddTransient<IChatbotClient, MockChatbotClient>();
    }
}
```
