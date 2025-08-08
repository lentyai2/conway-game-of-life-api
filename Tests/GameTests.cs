using GameOfLife.Game;

namespace GameOfLife.Tests;

[TestClass]
public sealed class GameTests
{
  [TestMethod]
  public void TestInitialEmptyStateFinal()
  {
    int[][] initialState = new int[][] { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };

    var board = new Board(initialState);
    board.NextGeneration();

    Assert.IsTrue(board.IsFinal);
  }

  [TestMethod]
  public void TestBlock()
  {
    int[][] initialState = new int[][] {
        new int[] { 0, 0, 0, 0 },
        new int[] { 0, 1, 1, 0 },
        new int[] { 0, 1, 1, 0 },
        new int[] { 0, 0, 0, 0 }
    };

    var board = new Board(initialState);
    board.NextGeneration(5);

    Assert.IsTrue(board.IsFinal);
    Assert.IsFalse(board.HasCycle);
  }

  [TestMethod]
  public void TestBlinker()
  {
    int[][] initialState = new int[][] {
        new int[] { 0, 0, 0, 0, 0 },
        new int[] { 0, 0, 1, 0, 0 },
        new int[] { 0, 0, 1, 0, 0 },
        new int[] { 0, 0, 1, 0, 0 },
        new int[] { 0, 0, 0, 0, 0 }
    };

    int[][] oscillator = new int[][] {
        new int[] { 0, 0, 0, 0, 0 },
        new int[] { 0, 0, 0, 0, 0 },
        new int[] { 0, 1, 1, 1, 0 },
        new int[] { 0, 0, 0, 0, 0 },
        new int[] { 0, 0, 0, 0, 0 }
    };

    var board = new Board(initialState);

    board.NextGeneration();
    Assert.AreEqual(board.CurrentState, new State(oscillator));

    board.NextGeneration();
    Assert.AreEqual(board.CurrentState, new State(initialState));

    board.FinalGeneration();
    Assert.AreEqual(board.CurrentState, new State(oscillator));
    Assert.IsTrue(board.HasCycle);
  }
}