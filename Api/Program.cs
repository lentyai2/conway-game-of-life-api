

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
    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();
    // app.MapGet("/game", async (GameService gameService) =>
    // {
    //     var games = await gameService.GetGames();
    //     return Results.Ok(games);
    // });

    app.MapGet("/game/{id:length(24)}", async (GameService gameService, string id) =>
    {
      var game = await gameService.GetAsync(id);
      if (game is null)
      {
        return Results.NotFound();
      }
      var denseMatrix = MatrixHelper.ToDenseMatrix(game.CurrentState.Select(c => new int[] { c.Row, c.Col }).ToArray());

      //Convert dense matrix in the form of a jagged array to a 2D array
      //and initialize the board object
      var board = new Board(denseMatrix);
      // Call NextGeneration on the board instance and return next generation state
      board.NextGeneration();
      // var jaggedArray = MatrixHelper.ToJaggedArray(board.CurrentState);
      return Results.Ok(board);
    });

    app.MapGet("/game_final/{id:length(24)}", async (GameService gameService, string id, [FromQuery] int n) =>
    {
      var game = await gameService.GetAsync(id);
      if (game is null)
      {
        return Results.NotFound();
      }
      var denseMatrix = MatrixHelper.ToDenseMatrix(game.CurrentState.Select(c => new int[] { c.Row, c.Col }).ToArray());

      //Convert dense matrix in the form of a jagged array to a 2D array
      //and initialize the board object
      var board = new Board(denseMatrix);
      // Call NextGeneration on the board instance and return next generation state
      board.NextGeneration(n, true);
      if (!board.IsFinal && !board.HasCycle) return Results.InternalServerError("Game is not final or has no cycle yet.");
      // var jaggedArray = MatrixHelper.ToJaggedArray(board.CurrentState);
      return Results.Ok(board);
    });

    // app.MapPost("/game", async (GameService gameService, int[][]initialState) =>
    // {
    //   var board = new Board(initialState);
    //   var newGame = new GameModel(board);
    //   await gameService.CreateAsync(newGame);
    //   return Results.Created($"/game/{newGame.Id}", newGame);
    // });
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    // app.UseAuthorization();
    // app.MapPost("/game", GameController.CreateGame);
    app.MapControllers();

    app.Run();
  }
}

public static class MatrixHelper
{
  public static int[][] ToDenseMatrix(int[][] sparseMatrix)
  {

    //Convert back to dense representation:
    // Initialize the dense list with zeros
    List<List<int>> denseMatrix = new List<List<int>>();
    for (int i = 0; i < 4; i++)
    {
      List<int> row = new List<int>(new int[4]); // Initialize row with zeros
      denseMatrix.Add(row);
    }

    // Populate the dense list with non-zero values from sparse data
    foreach (var item in sparseMatrix)
    {
      int row = item[0];
      int col = item[1];

      if (row >= 0 && row < 4 && col >= 0 && col < 4)
      {
        denseMatrix[row][col] = 1;
      }
      else
      {
        // Handle out-of-bounds index if necessary
        Console.WriteLine($"Warning: Sparse data contains out-of-bounds index ({row}, {col})");
      }
    }
    Console.WriteLine("Dense Matrix: " + denseMatrix);
    return denseMatrix.Select(r => r.ToArray()).ToArray();
  }

  // public static int[][] ToSparseMatrix(int[][] denseMatrix)
  // {

  //   // Convert to sparse array representation (List of tuples)
  //   List<(int row, int col)> sparseRepresentation = new List<(int row, int col)>();

  //   for (int r = 0; r < denseMatrix[0].Length; r++)
  //   {
  //     for (int c = 0; c < denseMatrix[r].Length; c++)
  //     {
  //       if (denseMatrix[r][c] != 0) // Only store non-zero elements
  //       {
  //         sparseRepresentation.Add((r, c));
  //       }
  //     }
  //   }
  //   return denseMatrix.Select(r => r.ToArray()).ToArray();
  // }
  // public static int[][] ToJaggedArray(int[,] matrix)
  // {
  //   int rows = matrix.GetLength(0);
  //   int cols = matrix.GetLength(1);
  //   int[][] jagged = new int[rows][];
  //   for (int i = 0; i < rows; i++)
  //   {
  //     jagged[i] = new int[cols];
  //     for (int j = 0; j < cols; j++)
  //     {
  //       jagged[i][j] = matrix[i, j];
  //     }
  //   }
  //   return jagged;
  // }
  // public static int[,] ToMatrix(int[][] jaggedArray)
  // {
  //   if (jaggedArray == null || jaggedArray.Length == 0)
  //   {
  //     return new int[0, 0];
  //   }
  //   int rows = jaggedArray.Length;
  //   int maxCols = 0;
  //   foreach (int[] innerArray in jaggedArray)
  //   {
  //     if (innerArray.Length > maxCols)
  //     {
  //       maxCols = innerArray.Length;
  //     }
  //   }
  //   int[,] twoDArray = new int[rows, maxCols];

  //   for (int i = 0; i < rows; i++)
  //   {
  //     for (int j = 0; j < jaggedArray[i].Length; j++)
  //     {
  //       twoDArray[i, j] = jaggedArray[i][j];
  //     }
  //   }
  //   return twoDArray;
  // }
}
