﻿using System;
using System.Threading;

using FlaUI.Core.AutomationElements;

namespace GoFigure.UiTests.Controls
{
  public class GameMenu
  {
    private readonly AutomationElement _element;

    public AutomationElement NewGame => _element.FindFirstDescendant(e => 
      e.ByAutomationId("StartNewGame")
    );

    public GameMenu(AutomationElement element) =>
      _element = element;

    public void OpenAndSelect(Func<GameMenu, AutomationElement> item)
    {
      _element.Click();

      Thread.Sleep(500);

      item?.Invoke(this)?.Click();
    }
  }
}
