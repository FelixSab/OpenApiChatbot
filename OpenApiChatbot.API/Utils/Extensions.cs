using Microsoft.AspNetCore.Mvc;

namespace OpenApiChatbot.API.Utils;

public static class Extensions
{
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
}
