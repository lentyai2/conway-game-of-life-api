using Game;

namespace Api;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.Configure<GameOfLifeDatabaseSettings>(
            builder.Configuration.GetSection("GameOfLifeDatabase")
        );

        builder.Services.AddSingleton<GameService>();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi(); // .NET 9.0 OpenAPI support

        var app = builder.Build();

        app.UseHttpsRedirection();

        // Optional: Add global exception handling middleware for better error responses
        app.UseExceptionHandler("/error");

        app.MapControllers();
        app.MapOpenApi(); // .NET 9.0 OpenAPI endpoint

        // Optional: Add a simple health check endpoint
        app.MapGet("/health", () => Results.Ok("Healthy"));

        app.Run();
    }
}