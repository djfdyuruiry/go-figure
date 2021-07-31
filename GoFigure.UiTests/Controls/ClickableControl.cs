using System;
using System.Threading;

using FlaUI.Core.AutomationElements;

namespace GoFigure.UiTests.Controls
{
  public class ClickableControl
  {
    protected readonly AutomationElement _element;

    public ClickableControl(AutomationElement element) => 
      _element = element;

    public void Click()
    {
      _element.Click();

      Thread.Sleep(500);
    }
  }
}
