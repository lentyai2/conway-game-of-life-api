using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using GameOfLife.Game;

namespace GameOfLife.Api;

public class GameModel
{
  // public GameModel() { CurrentState = new List<CellPosition>(); }

  public GameModel(Board board, List<CellPosition> sparseRepresentation)
  {
      CurrentState = sparseRepresentation;
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
  public int Generation { get; set; }

  [BsonElement("isFinal")]
  public bool IsFinal { get; set; }

  [BsonElement("hasCycle")]
  public bool HasCycle { get; set; }

}