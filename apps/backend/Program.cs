var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowLocalhost");

// Health check endpoint
app.MapGet("/api/health", () =>
    new { message = "Backend is healthy! Greetings from .NET 9" })
.WithName("Health")
.WithOpenApi();

// Sample API endpoint
app.MapGet("/api/hello", () =>
    new { message = "Hello from C# .NET 9 backend!" })
.WithName("Hello")
.WithOpenApi();

app.Run("http://localhost:5000");
