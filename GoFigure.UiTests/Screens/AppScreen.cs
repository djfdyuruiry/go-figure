using FlaUI.Core.AutomationElements;

using GoFigure.UiTests.Controls;

namespace GoFigure.UiTests.Screens
{
  public class AppScreen
  {
    private readonly Window _window;

    public MenuBar MenuBar => new MenuBar(
      _window.FindFirstChild(e => e.ByClassName("MenuBarView"))
    );

    public AutomationElement GameArea => _window.FindFirstChild(e => e.ByClassName("GameView"));

    public StatusControl Status => new StatusControl(
      GameArea.FindFirstChild(e => e.ByClassName("StatusView"))
    );

    public static AppScreen InWindow(Window window) =>
      new AppScreen(window);

    public AppScreen(Window window) =>
      _window = window;
  }
}
