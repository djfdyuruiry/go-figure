using System.Windows;

namespace GoFigure.App.Utils.Interfaces
{
  public interface IWindowLookup
  {
    Window WindowForElement(DependencyObject element);
  }
}
