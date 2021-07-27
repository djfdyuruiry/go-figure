using System.Threading;

using Xunit;

using GoFigure.UiTests.Screens;

namespace GoFigure.UiTests
{
  public class MenuBarTests : UiTestBase
  {
    //[Fact]
    [UiTest]
    public void When_NewGame_Is_Clicked_Then_Target_Is_Shown()
    {
      AppScreen.MenuBar.Game.OpenAndSelect(m => m.NewGame);

      Thread.Sleep(500);

      var targetNumber = AppScreen.Status.Target.Text;

      Assert.False(string.IsNullOrWhiteSpace(targetNumber));
    }
  }
}
