using System.Windows;
using GoFigure.App.Utils.Interfaces;
using static System.Windows.MessageBoxButton;
using static System.Windows.MessageBoxImage;

using static GoFigure.App.Constants;

namespace GoFigure.App.Utils
{
    public class MessageBoxManager : IMessageBoxManager
    {
        public void ShowInformation(Window window, string message) =>
            Show(window, message);

        public void ShowWarning(Window window, string message) =>
            Show(window, message, image: Warning);

        public MessageBoxResult ShowOkCancel(Window window, string message) =>
            Show(window, message, OKCancel);

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
