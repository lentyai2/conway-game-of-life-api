namespace GameOfLife.Game
{
    public class State
    {
      private readonly int[,] _state;
      private static readonly int[,] Neighbors = {
        { 0, 1 }, { 1, 0 },
        { 0, -1 }, { -1, 0 },
        { 1, 1 }, { 1, -1 },
        { -1, 1 }, { -1, -1 }
      };

      public State(int[,] initialState)
      {
        _state = initialState;
      }
  
      public State Next()
      {
        int rows = _state.GetLength(0);
        int cols = _state.GetLength(1);
        int[,] nextState = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
          for (int j = 0; j < cols; j++)
          {
            int liveNeighbors = CountLiveNeighbors(i, j, rows, cols);
            if (_state[i, j] == 1)
            {
              nextState[i, j] = (liveNeighbors < 2 || liveNeighbors > 3) ? 0 : 1;
            }
            else
            {
              nextState[i, j] = (liveNeighbors == 3) ? 1 : 0;
            }
          }
        }

        return new State(nextState);
      }

      private int CountLiveNeighbors(int x, int y, int rows, int cols)
      {
        int count = 0;
        for (int k = 0; k < Neighbors.GetLength(0); k++)
        {
          int newX = x + Neighbors[k, 0];
          int newY = y + Neighbors[k, 1];
          if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
          {
            count += _state[newX, newY];
          }
        }
        return count;
      }

      public override int GetHashCode()
      {
        var hashCode = 0;
        for (int i = 0; i < _state.GetLength(0); i++)
        {
          for (int j = 0; j < _state.GetLength(1); j++)
          {
            //TODO: verify that:
            // hashCode ^= _state[i, j] << (i * _state.GetLength(1) + j);
            HashCode.Combine(hashCode, _state[i, j].GetHashCode());
          }
        }
        return hashCode;
      }
      public override bool Equals(object? obj)
      {
        if (obj is State other)
        {
          if (_state.GetLength(0) != other._state.GetLength(0) || 
              _state.GetLength(1) != other._state.GetLength(1))
          {
            return false;
          }

          for (int i = 0; i < _state.GetLength(0); i++)
          {
            for (int j = 0; j < _state.GetLength(1); j++)
            {
              if (_state[i, j] != other._state[i, j])
              {
                return false;
              }
            }
          }
          return true;
        }
        return false;
      }

      public int this[int x, int y]
      {
        get => _state[x, y];
        set => _state[x, y] = value;
      }

      //TODO: verify that this is needed
      // public int[,] GetState()
      // {
      //   return _state;
      // }

      // public int Rows => _state.GetLength(0);
      // public int Columns => _state.GetLength(1);

      // public override string ToString()
      // {
      //   var result = new System.Text.StringBuilder();
      //   for (int i = 0; i < _state.GetLength(0); i++)
      //   {
      //     for (int j = 0; j < _state.GetLength(1); j++)
      //     {
      //       result.Append(_state[i, j] + " ");
      //     }
      //     result.AppendLine();
      //   }
      //   return result.ToString();
      // } 
    }
}
