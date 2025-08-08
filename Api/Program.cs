using GameOfLife.Game;
// using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLife.Api;
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
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    app.MapGet("/game/{id:length(24)}", async (GameService gameService, string id, [FromQuery] int? n) =>
    {
      var game = await gameService.GetAsync(id);
      if (game is null)
      {
        return Results.NotFound();
      }
      var denseMatrix = GameModel.ToDenseMatrix(game.CurrentState.Select(c => new int[] { c.Row, c.Col }).ToArray());

      var board = new Board(denseMatrix);

      // Call NextGeneration on the board instance and return next generation state
      board.NextGeneration(n ?? 1, true);

      if (n > 1 && !board.IsFinal && !board.HasCycle) return Results.InternalServerError("Game is not final or has no cycle yet.");

      return Results.Ok(board);
    });

    app.MapPost("/game", async (GameService gameService, [FromBody] int[][] initialState) =>
    {
      var board = new Board(initialState);
      var newGame = new GameModel(board);
      await gameService.CreateAsync(newGame);
      return Results.Created($"/game/{newGame.Id}", newGame);
    });
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.Run();
  }
}
