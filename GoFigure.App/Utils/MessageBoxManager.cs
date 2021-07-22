using System.Windows;

using static System.Windows.MessageBoxButton;
using static System.Windows.MessageBoxImage;

using GoFigure.App.Utils.Interfaces;

using static GoFigure.App.Constants;

namespace GoFigure.App.Utils
{
  public class MessageBoxManager : IMessageBoxManager
  {
    private readonly IWindowLookup _windowLookup;

    public MessageBoxManager(IWindowLookup windowLookup) => 
      _windowLookup = windowLookup;

    public void ShowInformation(DependencyObject element, string message) =>
      Show(_windowLookup.WindowForElement(element), message);

    public void ShowWarning(DependencyObject element, string message) =>
      Show(_windowLookup.WindowForElement(element), message, image: Warning);

    public MessageBoxResult ShowOkCancel(DependencyObject element, string message) =>
      Show(_windowLookup.WindowForElement(element), message, OKCancel);

    private MessageBoxResult Show(
      Window window,
      string message,
      MessageBoxButton button = OK,
      MessageBoxImage image = Information
    ) =>
      window is null
      ? MessageBox.Show(
        message,
        MessageBoxHeader,
        button,
        image
        )
      : MessageBox.Show(
        window,
        message,
        MessageBoxHeader,
        button,
        image
        );
  }
}
