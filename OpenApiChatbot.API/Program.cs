using Microsoft.AspNetCore.Mvc;
using OpenApiChatbot.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddChatbotClient();

// Add Swagger for testing
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Set up a global exception handler for unhandled exceptions, that points to the "/error" endpoint.
app.UseExceptionHandler("/error");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();