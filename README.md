# HttpClientHelper

A lightweight helper service that wraps `HttpClient` with strongly-typed methods for common HTTP operations, integrated logging, JSON (de)serialization, and Polly-based resilience policies.

---

## ✨ Features

- 🔹 Strongly-typed **GET**, **POST**, **PUT**, and **DELETE** requests  
- 🔹 Automatic **JSON serialization/deserialization** with `System.Text.Json`  
- 🔹 Built-in **request logging** with elapsed time measurement  
- 🔹 Supports posting **form-urlencoded data**  
- 🔹 Simplified retrieval of **access tokens**  
- 🔹 Polly-based **retry & circuit-breaker** resilience policies  

---

## 📦 Installation

Install via NuGet:

```bash
dotnet add package HttpClientLibrary
using HttpClientLibrary.HttpClientService;
using HttpClientLibrary.Samples; // for AddHttpClientHelper

var builder = WebApplication.CreateBuilder(args);

// Register HttpClientHelper with resilience
builder.Services.AddHttpClientHelper(client =>
{
    client.BaseAddress = new Uri("https://api.example.com");
});

var app = builder.Build();

builder.Services.AddHttpClientHelper(
    configureClient: client =>
    {
        client.BaseAddress = new Uri("https://api.example.com");
        client.DefaultRequestHeaders.Add("X-Api-Key", "secret-key");
    },
    retryDelays: new[]
    {
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(3),
        TimeSpan.FromSeconds(7)
    },
    exceptionsAllowedBeforeBreaking: 3,
    breakDuration: TimeSpan.FromSeconds(60)
);
flowchart LR
    A[Your Service / Controller] -->|calls| B[HttpClientHelper]
    B -->|wraps| C[HttpClient]
    C -->|executes| D[Polly Retry Policy]
    D --> E[Polly Circuit-Breaker Policy]
    E -->|sends request| F[Remote API]
var products = await _httpClientHelper.HttpRetrieveAllAsync<Product>("api/products");
