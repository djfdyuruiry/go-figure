using Xunit;

using GoFigure.UiTests.Screens;

namespace GoFigure.UiTests.Tests
{
  public class MenuBarTests : UiTestBase
  {
    //[Fact]
    [UiTest]
    public void When_NewGame_Is_Started_Then_Target_Is_Shown()
    {
      AppScreen.MenuBar
        .Game
        .StartNewGame();

      Assert.False(
        string.IsNullOrWhiteSpace(
          AppScreen.Status.Target.Value
        )
      );
    }
  }
}
