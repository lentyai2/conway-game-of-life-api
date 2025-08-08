using GameOfLife.Game;

namespace GameOfLife.Tests;

[TestClass]
public sealed class GameTests
{
  [TestMethod]
  public void TestInitialEmptyStateFinal()
  {
    var initialState = new[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

    var board = new Board(initialState);
    board.NextGeneration();

    Assert.IsTrue(board.IsFinal);
  }

  [TestMethod]
  public void TestBlock()
  {
    var initialState = new[,] {
        { 0, 0, 0, 0 },
        { 0, 1, 1, 0 },
        { 0, 1, 1, 0 },
        { 0, 0, 0, 0 }
    };

    var board = new Board(initialState);
    board.NextGeneration(5);

    Assert.IsTrue(board.IsFinal);
    Assert.IsFalse(board.HasCycle);
  }

  [TestMethod]
  public void TestBlinker()
  {
    var initialState = new[,] {
        { 0, 0, 0, 0, 0 },
        { 0, 0, 1, 0, 0 },
        { 0, 0, 1, 0, 0 },
        { 0, 0, 1, 0, 0 },
        { 0, 0, 0, 0, 0 }
    };

    var oscillator = new[,] {
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 1, 1, 1, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 }
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