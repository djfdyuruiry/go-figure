using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace GoFigure.UiTests.Controls
{
  public class MenuBar
  {
    private readonly AutomationElement _element;

    public GameMenu Game => new GameMenu(
      _element.FindFirstChild(e => e.ByName("Game"))
    );

    public MenuBar(AutomationElement element) =>
      _element = element.FindFirstChild(e => e.ByControlType(ControlType.Menu));
  }
}
