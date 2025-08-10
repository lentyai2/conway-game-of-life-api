namespace Game;

public class Board
{
  public State CurrentState { get; private set; }
  public string BoardId { get; private set; } = Guid.NewGuid().ToString();
  public int Generation { get; private set; } = 0;

  public bool IsFinal { get; private set; } = false;

  public bool HasCycle { get; private set; } = false;
  private HashSet<string> _previousStates = new HashSet<string>();
  private readonly int[][] _initialState;

  public Board(int[][] initialState)
  {
    _initialState = initialState;
    CurrentState = new State(initialState);
  }

  public void NextGeneration(int maxGenerations = 1, bool stopWithCycle = false)
  {
    for (int i = 0; i < maxGenerations && !IsFinal && !(stopWithCycle && HasCycle); i++)
    {
      var currentStateHash = CurrentState.GetHashCode().ToString();
      if (_previousStates.Contains(currentStateHash))
      {
        HasCycle = true;
        if (stopWithCycle) break;
      }
      _previousStates.Add(currentStateHash);

      var nextState = CurrentState.Next();
      if (nextState.Equals(CurrentState))
      {
        IsFinal = true;
        break;
      }

      CurrentState = nextState;
      Generation++;
    }
  }
}

