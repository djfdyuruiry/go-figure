using System.Windows;

using static System.Windows.MessageBoxButton;
using static System.Windows.MessageBoxImage;

using static GoFigure.App.Constants;

namespace GoFigure.App.Utils
{
    public class MessageBoxManager : IMessageBoxManager
    {
        public void ShowInformation(string message) =>
            Show(message);

        public void ShowWarning(string message) =>
            Show(message, image: Warning);

        public MessageBoxResult ShowOkCancel(string message) =>
            Show(message, OKCancel);

        private MessageBoxResult Show(
            string message,
            MessageBoxButton button = OK,
            MessageBoxImage image = Information
        ) =>
            MessageBox.Show(
                message,
                MessageBoxHeader,
                button,
                image
            );
    }
}
