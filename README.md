# OpenApiChatbot Project

This project provides a modular and reusable .NET API solution to interact with a chatbot interface defined by an OpenAPI specification. It includes:

- **Contracts:** DTOs and interfaces that define the data structures and client abstractions.
- **API Layer:** Controllers and endpoints for creating chats and submitting chat completions.

## Key Features

- **Create and Manage Chats:** Start a new chat session with a bot using a POST request.
- **Chat Completion:** Send user messages to an existing chat and receive a response from the assistant.
- **Reusability:** The interface `IChatbotClient` can be implemented by different backends, making it easy to integrate into various environments.
- **Validation and Error Handling:** Data annotations ensure validation, and standardized error responses (`ProblemDetails`) improve client-side error handling.

## Getting Started

### Prerequisites

- [.NET 9 or later](https://dotnet.microsoft.com/en-us/download)
- A running environment that can host an ASP.NET Core Web API (e.g., local machine, Docker).

### Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/FelixSab/OpenApiChatbot.git
   cd OpenApiChatbot
