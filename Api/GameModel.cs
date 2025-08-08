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

  public static List<CellPosition> ToSparseMatrix(int[][] initialState)
  {
    List<CellPosition> sparseRepresentation = [];
    if (initialState == null || initialState.Length == 0)
      return sparseRepresentation;
    for (int r = 0; r < initialState.Length; r++)
    {
      for (int c = 0; c < initialState[r].Length; c++)
      {
        if (initialState[r][c] != 0) // Only store non-zero elements
        {
          sparseRepresentation.Add(new CellPosition { Row = r, Col = c });
        }
      }
    }
    return sparseRepresentation;
  }
}