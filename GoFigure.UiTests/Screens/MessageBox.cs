using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

using GoFigure.UiTests.Controls;

namespace GoFigure.UiTests.Screens
{
  public class MessageBox
  {
    private readonly Window _window;

    public ClickableControl Ok => new ClickableControl(
      _window.FindFirstDescendant(e => e.ByName("OK"))
    );

    public ControlWithValue Prompt => new ControlWithValue(
      _window.FindFirstDescendant(e => e.ByControlType(ControlType.Text))
    );

    public MessageBox(Window window) => 
      _window = window;
  }
}
