using FlaUI.Core.AutomationElements;

namespace GoFigure.UiTests.Controls
{
  public class TextBlock
  {
    private readonly AutomationElement _element;

    public string Text => _element.Name;

    public TextBlock(AutomationElement element) => 
      _element = element;
  }
}
