using GoFigure.UiTests.Screens;
using Xunit;

namespace GoFigure.UiTests
{
  public class MenuBarTests : UiTestBase
  {
    //[Fact]
    [VideoRecorded]
    public void When_NewGame_Is_Clicked_Then_Target_Is_Shown()
    {
      var appScreen = AppScreen.InWindow(GetMainWindow());

      appScreen.MenuBar
        .Game
        .OpenAndSelect(m => m.NewGame);

      var targetNumber = appScreen.Status
        .Target
        .Text;

      Assert.False(string.IsNullOrWhiteSpace(targetNumber));
    }
  }
}
