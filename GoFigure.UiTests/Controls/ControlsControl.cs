using System.Collections.Generic;

using FlaUI.Core.AutomationElements;

namespace GoFigure.UiTests.Controls
{
  public class ControlsControl
  {
    private readonly AutomationElement _element;

    public Dictionary<string, ClickableControlWithValue> NumberToControl
    {
      get
      {
        var dict = new Dictionary<string, ClickableControlWithValue>();

        dict[Number1.Value] = Number1;
        dict[Number2.Value] = Number2;
        dict[Number3.Value] = Number3;
        dict[Number4.Value] = Number4;

        return dict;
      }
    }

    public Dictionary<string, ClickableControlWithValue> OperatorToControl
    { 
      get
      {
        var dict = new Dictionary<string, ClickableControlWithValue>();

        dict[Multiply.Value] = Multiply;
        dict[Divide.Value] = Divide;
        dict[Plus.Value] = Plus;
        dict[Minus.Value] = Minus;

        return dict;
      }
    }

    public ClickableControlWithValue Number1 => new ClickableControlWithValue(
      _element.FindFirstDescendant(e => e.ByAutomationId("Number1"))
    );

    public ClickableControlWithValue Number2 => new ClickableControlWithValue(
      _element.FindFirstDescendant(e => e.ByAutomationId("Number2"))
    );

    public ClickableControlWithValue Number3 => new ClickableControlWithValue(
      _element.FindFirstDescendant(e => e.ByAutomationId("Number3"))
    );

    public ClickableControlWithValue Number4 => new ClickableControlWithValue(
      _element.FindFirstDescendant(e => e.ByAutomationId("Number4"))
    );

    public ClickableControlWithValue Multiply => new ClickableControlWithValue(
      _element.FindFirstDescendant(e => e.ByAutomationId("Multiply"))
    );

    public ClickableControlWithValue Divide => new ClickableControlWithValue(
      _element.FindFirstDescendant(e => e.ByAutomationId("Divide"))
    );

    public ClickableControlWithValue Plus => new ClickableControlWithValue(
      _element.FindFirstDescendant(e => e.ByAutomationId("Plus"))
    );

    public ClickableControlWithValue Minus => new ClickableControlWithValue(
      _element.FindFirstDescendant(e => e.ByAutomationId("Minus"))
    );

    public ClickableControl SubmitSolution => new ClickableControl(
      _element.FindFirstDescendant(e => e.ByAutomationId("SubmitSolution"))
    );

    public ControlsControl(AutomationElement element) => 
      _element = element;

    public void EnterCorrectSolution(DebugEventListener listener)
    {
      var currentSolution = listener.CurrentSolution;

      for (int i = 0; i < currentSolution.Count; i++)
      {
        var numberOrOperator = currentSolution[i];
        var isNumber = i == 0 || i % 2 == 0;

        if (isNumber)
        {
          NumberToControl[numberOrOperator].Click();
        }
        else
        {
          OperatorToControl[numberOrOperator].Click();
        }
      }
    }

    public void SubmitCurrentSolution() =>
      SubmitSolution.Click();
  }
}
