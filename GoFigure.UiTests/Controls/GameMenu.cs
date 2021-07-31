using FlaUI.Core.AutomationElements;

namespace GoFigure.UiTests.Controls
{
  public class GameMenu : ClickableControl
  {
    public AutomationElement NewGame => _element.FindFirstDescendant(e => 
      e.ByAutomationId("StartNewGame")
    );

    public GameMenu(AutomationElement element) : base(element)
    {
    }

    public void StartNewGame()
    {
      Click();
      NewGame.Click();
    }
  }
}
