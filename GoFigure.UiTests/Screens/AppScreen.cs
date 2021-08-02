using System.Linq;
using FlaUI.Core.AutomationElements;

using GoFigure.UiTests.Controls;

namespace GoFigure.UiTests.Screens
{
  public class AppScreen : WithUtil<AppScreen>
  {
    private readonly Window _window;
    private readonly DebugEventListener _eventListener;

    public MenuBar MenuBar => new MenuBar(
      _window.FindFirstChild(e => e.ByClassName("MenuBarView"))
    );

    public AutomationElement GameArea => 
      _window.FindFirstChild(e => e.ByClassName("GameView"));

    public StatusControl Status => new StatusControl(
      GameArea.FindFirstChild(e => e.ByClassName("StatusView"))
    );

    public ControlsControl Controls => new ControlsControl(
      GameArea.FindFirstChild(e => e.ByClassName("ControlsView")),
      _eventListener
    );

    public MessageBox MessageBox => new MessageBox(
      _window.ModalWindows.FirstOrDefault()
    );

    public static AppScreen InWindow(Window window) =>
      new AppScreen(window);

    public AppScreen(Window window)
    {
      _window = window;
      _self = this;
    }
  }
}
