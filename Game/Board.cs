namespace GameOfLife.Game;

public class Board(int[,] initialState)
{
  // public Guid BoardId { get;  private set; } = Guid.NewGuid();
  public State CurrentState { get; private set; } = new State(initialState);
  public int Generation { get; private set; } = 0;

  public bool IsFinal { get; private set; } = false;

  public bool HasCycle { get; private set; } = false;
  private HashSet<string> _previousStates = new HashSet<string>();

  public void NextGeneration(int? maxGenerations = 1)
  {
    for (int i = 0; i < maxGenerations && !IsFinal; i++)
    {
      // Check if the next state is the same as the current state
      if (CurrentState.Next().Equals(CurrentState))
      {
        IsFinal = true;
        return;
      }

      // Update the current state and increment the generation count          
      CurrentState = CurrentState.Next();
      Generation++;
    }
  }

  public void FinalGeneration()
  {
    while (!IsFinal && !HasCycle)
    {
      string currentStateHash = CurrentState.GetHashCode().ToString();
      // Check if the current state has already been seen
      if (_previousStates.Contains(currentStateHash))
      {
        HasCycle = true;
        return;
      }

      _previousStates.Add(currentStateHash);
      // Check if the next state is the same as the current state
      if (CurrentState.Next().Equals(CurrentState))
      {
        IsFinal = true;
        return;
      }

      // Update the current state and increment the generation count          
      CurrentState = CurrentState.Next();
      Generation++;
    }
  }

  public static int[,] ToMatrix(int[][] jaggedArray)
  {

    if (jaggedArray == null || jaggedArray.Length == 0)
    {
      return new int[0, 0];
    }
    int rows = jaggedArray.Length;
    int maxCols = 0;
    foreach (int[] innerArray in jaggedArray)
    {
      if (innerArray.Length > maxCols)
      {
        maxCols = innerArray.Length;
      }
    }

    int[,] twoDArray = new int[rows, maxCols];

    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < jaggedArray[i].Length; j++)
      {
        twoDArray[i, j] = jaggedArray[i][j];
      }
    }

    return twoDArray;
  }

  public static List<CellPosition> ToSparseMatrix(int[][] initialState)
  {
    List<CellPosition> sparseRepresentation = [];

    for (int r = 0; r < initialState[0].Length; r++)
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

  // public override string ToString()
  // {
  //   return $"BoardId: {BoardId}, Generation: {Generation}, IsFinal: {IsFinal}, HasCycle: {HasCycle}";
  // }

  // public override int GetHashCode()
  // {
  //   return BoardId.GetHashCode();
  // }

  // public override bool Equals(object? obj)
  // {
  //   if (obj is Board otherBoard)
  //   {
  //     return BoardId.Equals(otherBoard.BoardId);
  //   }
  //   return false;
  // }

  // private static int[,] ToMatrix(int[][] jaggedArray)
  // {
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

