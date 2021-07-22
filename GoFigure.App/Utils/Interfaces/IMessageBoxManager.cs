using System.Windows;

namespace GoFigure.App.Utils.Interfaces
{
  public interface IMessageBoxManager
  {
    void ShowInformation(DependencyObject element, string message);

    MessageBoxResult ShowOkCancel(DependencyObject element, string message);

    void ShowWarning(DependencyObject element, string message);
  }
}
