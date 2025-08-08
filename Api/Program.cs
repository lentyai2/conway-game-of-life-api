

using GameOfLife.Game;
using Microsoft.AspNetCore.Builder;
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
    // builder.Services.AddControllers();
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

    app.MapPost("/game", async (GameService gameService, int[][]initialState) =>
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

// public static class MatrixHelper
// {
//   public static int[][] ToDenseMatrix(int[][] sparseMatrix)
//   {
//     //Convert back to dense representation:
//     // Initialize the dense list with zeros
//     var denseMatrix = new List<List<int>>();
//     for (int i = 0; i < 4; i++)
//     {
//       List<int> row = new List<int>(new int[4]); // Initialize row with zeros
//       denseMatrix.Add(row);
//     }

//     // Populate the dense list with non-zero values from sparse data
//     foreach (var item in sparseMatrix)
//     {
//       int row = item[0];
//       int col = item[1];

//       if (row >= 0 && row < 4 && col >= 0 && col < 4)
//       {
//         denseMatrix[row][col] = 1;
//       }
//       else
//       {
//         // Handle out-of-bounds index if necessary
//         Console.WriteLine($"Warning: Sparse data contains out-of-bounds index ({row}, {col})");
//       }
//     }
//     return denseMatrix.Select(r => r.ToArray()).ToArray();
//   }
// }
