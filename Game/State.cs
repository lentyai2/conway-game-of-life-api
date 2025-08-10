namespace Game
{
  public class State
  {
    private readonly int[][] _state;
    private static readonly int[][] Neighbors = new int[][] {
      new int[] { 0, 1 }, new int[] { 1, 0 },
      new int[] { 0, -1 }, new int[] { -1, 0 },
      new int[] { 1, 1 }, new int[] { 1, -1 },
      new int[] { -1, 1 }, new int[] { -1, -1 }
    };

    public int[][] Matrix { get; private set; }
    public State(int[][] initialState)
    {
      _state = initialState;
      Matrix = initialState; // Initialize Matrix with the initial state
    }
  
      public State Next()
      {
        int rows = _state.Length;
        int cols = rows > 0 ? _state[0].Length : 0;
        int[][] nextState = new int[rows][];
        for (int i = 0; i < rows; i++)
        {
          nextState[i] = new int[cols];
          for (int j = 0; j < cols; j++)
          {
            int liveNeighbors = CountLiveNeighbors(i, j, rows, cols);
            if (_state[i][j] == 1)
            {
              nextState[i][j] = (liveNeighbors < 2 || liveNeighbors > 3) ? 0 : 1;
            }
            else
            {
              nextState[i][j] = (liveNeighbors == 3) ? 1 : 0;
            }
          }
        }
        Matrix = nextState; // Update the Matrix with the new state
        // Return a new State instance with the next state
        // This is necessary to maintain immutability of the State class
        return new State(nextState);
      }

      private int CountLiveNeighbors(int x, int y, int rows, int cols)
      {
        int count = 0;
        for (int k = 0; k < Neighbors.Length; k++)
        {
          int newX = x + Neighbors[k][0];
          int newY = y + Neighbors[k][1];
          if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
          {
            count += _state[newX][newY];
          }
        }
        return count;
      }

      public override int GetHashCode()
      {
        var hashCode = 0;
        for (int i = 0; i < _state.Length; i++)
        {
          for (int j = 0; j < _state[i].Length; j++)
          {
            hashCode = HashCode.Combine(hashCode, _state[i][j].GetHashCode());
          }
        }
        return hashCode;
      }
      public override bool Equals(object? obj)
      {
        if (obj is State other)
        {
          if (_state.Length != other._state.Length)
            return false;
          for (int i = 0; i < _state.Length; i++)
          {
            if (_state[i].Length != other._state[i].Length)
              return false;
            for (int j = 0; j < _state[i].Length; j++)
            {
              if (_state[i][j] != other._state[i][j])
                return false;
            }
          }
          return true;
        }
        return false;
      }

      public int this[int x, int y]
      {
        get => _state[x][y];
        set => _state[x][y] = value;
      }

      //TODO: verify that this is needed
      // public int[][] GetState()
      // {
      //   return _state;
      // }

      // public int Rows => _state.Length;
      // public int Columns => _state.Length > 0 ? _state[0].Length : 0;

      // public override string ToString()
      // {
      //   var result = new System.Text.StringBuilder();
      //   for (int i = 0; i < _state.Length; i++)
      //   {
      //     for (int j = 0; j < _state[i].Length; j++)
      //     {
      //       result.Append(_state[i][j] + " ");
      //     }
      //     result.AppendLine();
      //   }
      //   return result.ToString();
      // } 
    }
}
