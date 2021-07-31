using System;
using Xunit;

namespace GoFigure.UiTests.Tests
{
  public class ControlsTests : UiTestBase
  {
    //[Fact]
    [UiTest]
    public void When_NewGame_Is_Started_And_Correct_Solution_Entered_Then_Success_Popup_Is_Shown()
    {
      AppScreen.MenuBar.Game.StartNewGame();
      AppScreen.Controls.EnterCorrectSolution(EventListener);
      AppScreen.Controls.SubmitCurrentSolution();

      Assert.Equal(
        "Solution correct! Moving to next level",
        AppScreen.MessageBox.Prompt.Value
      );
    }
  }
}
