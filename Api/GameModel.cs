using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using GameOfLife.Game;

namespace GameOfLife.Api;

public class GameModel
{
  // public GameModel() { CurrentState = new List<CellPosition>(); }

  public GameModel(Board board)
  {
    CurrentState = ToSparseMatrix(board.CurrentState.Matrix);
    Generation = board.Generation;
    IsFinal = board.IsFinal;
    HasCycle = board.HasCycle;
  }
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? Id { get; set; }

  [BsonElement("currentState")]
  public List<CellPosition> CurrentState { get; set; } = new List<CellPosition>();

  [BsonElement("generation")]
  public int Generation { get; set; } = 0;

  [BsonElement("isFinal")]
  public bool IsFinal { get; set; } = false;

  [BsonElement("hasCycle")]
  public bool HasCycle { get; set; } = false;

  public static List<CellPosition> ToSparseMatrix(int[][] denseMatrix)
  {
    List<CellPosition> sparseRepresentation = [];
    if (denseMatrix == null || denseMatrix.Length == 0)
      return sparseRepresentation;
    for (int r = 0; r < denseMatrix.Length; r++)
    {
      for (int c = 0; c < denseMatrix[r].Length; c++)
      {
        if (denseMatrix[r][c] != 0) // Only store non-zero elements
        {
          sparseRepresentation.Add(new CellPosition { Row = r, Col = c });
        }
      }
    }
    return sparseRepresentation;
  }

  public static int[][] ToDenseMatrix(int[][] sparseMatrix)
  {
    //Convert back to dense representation:
    // Initialize the dense list with zeros
    var denseMatrix = new List<List<int>>();
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
    return denseMatrix.Select(r => r.ToArray()).ToArray();
  }
}