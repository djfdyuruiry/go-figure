using System.Windows;

using GoFigure.App.Utils.Interfaces;

namespace GoFigure.App.Utils
{
  public class WindowLookup : IWindowLookup
  {
    public Window WindowForElement(DependencyObject element) =>
      Window.GetWindow(element);
  }
}
