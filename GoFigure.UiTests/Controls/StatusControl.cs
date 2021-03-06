using FlaUI.Core.AutomationElements;

namespace GoFigure.UiTests.Controls
{
  public class StatusControl
  {
    private readonly AutomationElement _element;

    public ControlWithValue Target => new ControlWithValue(
      _element.FindFirstDescendant(e => e.ByAutomationId("Target"))
    );

    public StatusControl(AutomationElement element) => 
      _element = element;
  }
}
