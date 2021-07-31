using FlaUI.Core.AutomationElements;

namespace GoFigure.UiTests.Controls
{
  public class ClickableControlWithValue : ClickableControl
  {
    public string Value => _element.Name;

    public ClickableControlWithValue(AutomationElement element) : base(element) {}       
  }
}
