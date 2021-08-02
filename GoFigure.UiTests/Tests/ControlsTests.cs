using Xunit;

namespace GoFigure.UiTests.Tests
{
  public class LevelMeterTests : UiTestBase
  {
    [Fact]
    [UiTest]
    public void When_NewGame_Is_Started_And_Correct_Solution_Entered_Then_Level_Meter_Updates() =>
      AppScreen.Use(s =>
      {
        s.MenuBar.Game.StartNewGame();

        s.Controls.Use(c =>
        {
          c.EnterCorrectSolution();
          c.SubmitCurrentSolution();
        });

        s.MessageBox.Ok.Click();
      });
  }
}
