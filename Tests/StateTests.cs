using GameOfLife.Game;

namespace GameOfLife.Tests;

[TestClass]
public sealed class StateTests
{
  [TestMethod]
  public void TestInitialStateAndEquals()
  {
    var initialState = new[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

    var state = new State(initialState);
    var another = new State(initialState);

    Assert.AreEqual(state.GetHashCode(), another.GetHashCode());
    Assert.AreEqual(state, another);
  }

  [TestMethod]
  public void TestDeath()
  {
    var initialState = new State(new[,] { { 0, 0 }, { 0, 1 } });
    var state = initialState.Next();

    var shouldBe = new State(new[,] { { 0, 0 }, { 0, 0 } });
    Assert.AreEqual(state, shouldBe);
  }

  [TestMethod]
  public void TestLife()
  {
    var initialState = new State(new[,] { { 1, 1 }, { 1, 0 } });
    var state = initialState.Next();

    var shouldBe = new State(new[,] { { 1, 1 }, { 1, 1 } });
    Assert.AreEqual(state, shouldBe);        
  }
}
